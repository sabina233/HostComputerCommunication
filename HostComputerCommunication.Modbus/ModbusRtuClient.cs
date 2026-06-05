using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.Modbus.Protocol;

namespace HostComputerCommunication.Modbus;

/// <summary>
/// Modbus RTU 客户端
/// </summary>
public class ModbusRtuClient : IDisposable
{
    private readonly SerialPort.ISerialPort _serialPort;
    private readonly Logger _logger;
    private readonly ModbusConfig _config;
    private readonly object _lock = new();
    private bool _disposed;

    private byte[]? _responseBuffer;
    private readonly AutoResetEvent _responseReceived = new(false);

    public bool IsConnected => _serialPort.IsConnected;
    public event EventHandler<ModbusResponse>? ResponseReceived;

    public ModbusRtuClient(SerialPort.ISerialPort serialPort, Logger logger, ModbusConfig config)
    {
        _serialPort = serialPort;
        _logger = logger;
        _config = config;
        _serialPort.DataReceived += OnSerialDataReceived;
    }

    public ModbusResponse? ReadHoldingRegisters(ushort startAddress, ushort quantity)
    {
        byte[] request = ModbusFrameBuilder.RtuReadHoldingRegisters(_config.SlaveAddress, startAddress, quantity);
        return SendAndParse(request);
    }

    public ModbusResponse? ReadInputRegisters(ushort startAddress, ushort quantity)
    {
        byte[] request = ModbusFrameBuilder.RtuReadInputRegisters(_config.SlaveAddress, startAddress, quantity);
        return SendAndParse(request);
    }

    public ModbusResponse? ReadCoils(ushort startAddress, ushort quantity)
    {
        byte[] request = ModbusFrameBuilder.RtuReadCoils(_config.SlaveAddress, startAddress, quantity);
        return SendAndParse(request);
    }

    public ModbusResponse? ReadDiscreteInputs(ushort startAddress, ushort quantity)
    {
        byte[] request = ModbusFrameBuilder.RtuReadDiscreteInputs(_config.SlaveAddress, startAddress, quantity);
        return SendAndParse(request);
    }

    public ModbusResponse? WriteSingleRegister(ushort address, ushort value)
    {
        byte[] request = ModbusFrameBuilder.RtuWriteSingleRegister(_config.SlaveAddress, address, value);
        return SendAndParse(request);
    }

    public ModbusResponse? WriteMultipleRegisters(ushort startAddress, ushort[] values)
    {
        byte[] request = ModbusFrameBuilder.RtuWriteMultipleRegisters(_config.SlaveAddress, startAddress, values);
        return SendAndParse(request);
    }

    public ModbusResponse? WriteSingleCoil(ushort address, bool value)
    {
        byte[] request = ModbusFrameBuilder.RtuWriteSingleCoil(_config.SlaveAddress, address, value);
        return SendAndParse(request);
    }

    public ModbusResponse? WriteMultipleCoils(ushort startAddress, bool[] values)
    {
        byte[] request = ModbusFrameBuilder.RtuWriteMultipleCoils(_config.SlaveAddress, startAddress, values);
        return SendAndParse(request);
    }

    /// <summary>
    /// 发送原始帧并解析响应
    /// </summary>
    private ModbusResponse? SendAndParse(byte[] request)
    {
        lock (_lock)
        {
            for (int retry = 0; retry <= _config.Retries; retry++)
            {
                try
                {
                    _responseBuffer = null;
                    _serialPort.Send(request);

                    _logger.Debug($"发送: {ByteHelper.ToHexString(request)}", nameof(ModbusRtuClient));

                    if (_responseReceived.WaitOne(_config.Timeout))
                    {
                        if (_responseBuffer != null)
                        {
                            var response = ModbusResponseParser.ParseRtuResponse(_responseBuffer);
                            ResponseReceived?.Invoke(this, response);
                            if (response.Success) return response;

                            _logger.Warning($"响应异常: {response.ErrorMessage}", nameof(ModbusRtuClient));
                        }
                    }
                    else
                    {
                        _logger.Warning($"等待响应超时，重试 {retry + 1}/{_config.Retries + 1}", nameof(ModbusRtuClient));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"通信异常: {ex.Message}", nameof(ModbusRtuClient));
                }
            }
            return null;
        }
    }

    private void OnSerialDataReceived(object? sender, byte[] data)
    {
        _responseBuffer = data;
        _responseReceived.Set();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _serialPort.DataReceived -= OnSerialDataReceived;
            _responseReceived.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
