using System.Collections.Concurrent;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.Modbus.Protocol;
using HostComputerCommunication.TcpSocket;

namespace HostComputerCommunication.Modbus;

/// <summary>
/// Modbus TCP 客户端
/// 通过以太网（TCP/IP）与 Modbus 设备通信
/// 使用 MBAP 头部协议，支持异步请求/响应
/// </summary>
public class ModbusTcpClient : IDisposable
{
    private readonly TcpClientManager _tcpClient;
    private readonly Logger _logger;
    private readonly ModbusConfig _config;
    private int _transactionId;
    private bool _disposed;

    private readonly ConcurrentDictionary<ushort, TaskCompletionSource<byte[]>> _pendingRequests = new();

    public bool IsConnected => _tcpClient.IsConnected;
    public event EventHandler<ModbusResponse>? ResponseReceived;

    public ModbusTcpClient(TcpClientManager tcpClient, Logger logger, ModbusConfig config)
    {
        _tcpClient = tcpClient;
        _logger = logger;
        _config = config;
        _tcpClient.DataReceived += OnTcpDataReceived;
    }

    public async Task<bool> ConnectAsync()
    {
        return await _tcpClient.ConnectAsync();
    }

    public void Disconnect()
    {
        _tcpClient.Disconnect();
    }

    public async Task<ModbusResponse?> ReadHoldingRegistersAsync(ushort startAddress, ushort quantity)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpReadHoldingRegisters(tid, _config.SlaveAddress, startAddress, quantity);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    public async Task<ModbusResponse?> ReadInputRegistersAsync(ushort startAddress, ushort quantity)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpReadInputRegisters(tid, _config.SlaveAddress, startAddress, quantity);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    public async Task<ModbusResponse?> ReadCoilsAsync(ushort startAddress, ushort quantity)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpReadCoils(tid, _config.SlaveAddress, startAddress, quantity);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    public async Task<ModbusResponse?> ReadDiscreteInputsAsync(ushort startAddress, ushort quantity)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpReadDiscreteInputs(tid, _config.SlaveAddress, startAddress, quantity);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    public async Task<ModbusResponse?> WriteSingleRegisterAsync(ushort address, ushort value)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpWriteSingleRegister(tid, _config.SlaveAddress, address, value);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    public async Task<ModbusResponse?> WriteMultipleRegistersAsync(ushort startAddress, ushort[] values)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpWriteMultipleRegisters(tid, _config.SlaveAddress, startAddress, values);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    public async Task<ModbusResponse?> WriteSingleCoilAsync(ushort address, bool value)
    {
        ushort tid = GetNextTransactionId();
        byte[] request = ModbusFrameBuilder.TcpWriteSingleCoil(tid, _config.SlaveAddress, address, value);
        byte[]? response = await SendRequestAsync(tid, request);
        return response != null ? ModbusResponseParser.ParseTcpResponse(response) : null;
    }

    private async Task<byte[]?> SendRequestAsync(ushort transactionId, byte[] request)
    {
        var tcs = new TaskCompletionSource<byte[]>();
        _pendingRequests[transactionId] = tcs;

        _logger.Debug($"发送 Modbus TCP 请求: {ByteHelper.ToHexString(request)}", nameof(ModbusTcpClient));

        if (!await _tcpClient.SendAsync(request))
        {
            _pendingRequests.TryRemove(transactionId, out _);
            return null;
        }

        using var cts = new CancellationTokenSource(_config.Timeout);
        cts.Token.Register(() => tcs.TrySetCanceled());

        try
        {
            return await tcs.Task;
        }
        catch (OperationCanceledException)
        {
            _logger.Warning($"请求超时 (TID: {transactionId})", nameof(ModbusTcpClient));
            _pendingRequests.TryRemove(transactionId, out _);
            return null;
        }
    }

    private void OnTcpDataReceived(object? sender, byte[] data)
    {
        _logger.Debug($"接收 Modbus TCP 响应: {ByteHelper.ToHexString(data)}", nameof(ModbusTcpClient));

        if (data.Length >= 7)
        {
            ushort transactionId = (ushort)((data[0] << 8) | data[1]);
            if (_pendingRequests.TryRemove(transactionId, out var tcs))
            {
                tcs.TrySetResult(data);
            }

            var response = ModbusResponseParser.ParseTcpResponse(data);
            ResponseReceived?.Invoke(this, response);
        }
    }

    private ushort GetNextTransactionId()
    {
        return (ushort)Interlocked.Increment(ref _transactionId);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _tcpClient.DataReceived -= OnTcpDataReceived;
            _tcpClient.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
