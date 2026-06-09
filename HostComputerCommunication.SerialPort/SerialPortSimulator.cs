using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;

namespace HostComputerCommunication.SerialPort;

/// <summary>
/// 串口模拟器
/// 用于无实物设备时自测，支持三种模拟模式：
/// 1. Loopback（回环）: 发送的数据原样返回
/// 2. RandomResponse（随机响应）: 返回随机数据
/// 3. ModbusSlave（Modbus从站）: 按 Modbus 协议返回寄存器数据
/// </summary>
public class SerialPortSimulator : ISerialPort
{
    private readonly Logger _logger;
    private bool _disposed;
    private readonly Random _random = new();

    public event EventHandler<byte[]>? DataReceived;
    public event EventHandler<SerialDataEventArgs>? DataTransferred;
    public event EventHandler<bool>? ConnectionStateChanged;

    public bool IsConnected { get; private set; }
    public SerialPortConfig Config { get; private set; }
    public long BytesSent { get; private set; }
    public long BytesReceived { get; private set; }

    /// <summary>
    /// 模拟模式
    /// </summary>
    public SimulationMode Mode { get; set; } = SimulationMode.Loopback;

    /// <summary>
    /// 模拟延迟（毫秒）
    /// </summary>
    public int SimulatedDelay { get; set; } = 50;

    public SerialPortSimulator(Logger logger)
    {
        _logger = logger;
        Config = new SerialPortConfig();
    }

    public bool Open(SerialPortConfig config)
    {
        Config = config;
        IsConnected = true;
        BytesSent = 0;
        BytesReceived = 0;

        _logger.Info($"[模拟] 串口 {config.PortName} 已打开 ({config.BaudRate},{config.DataBits},{config.Parity},{config.StopBits})", nameof(SerialPortSimulator));
        ConnectionStateChanged?.Invoke(this, true);
        return true;
    }

    public void Close()
    {
        if (IsConnected)
        {
            IsConnected = false;
            _logger.Info($"[模拟] 串口已关闭", nameof(SerialPortSimulator));
            ConnectionStateChanged?.Invoke(this, false);
        }
    }

    public bool Send(byte[] data)
    {
        if (!IsConnected)
        {
            _logger.Warning("[模拟] 串口未连接，无法发送数据", nameof(SerialPortSimulator));
            return false;
        }

        BytesSent += data.Length;
        DataTransferred?.Invoke(this, new SerialDataEventArgs(data, true));
        _logger.Debug($"[模拟] 发送 {data.Length} 字节: {ByteHelper.ToHexString(data)}", nameof(SerialPortSimulator));

        switch (Mode)
        {
            case SimulationMode.Loopback:
                _ = Task.Run(async () =>
                {
                    await Task.Delay(SimulatedDelay);
                    SimulateReceive(data);
                });
                break;

            case SimulationMode.RandomResponse:
                _ = Task.Run(async () =>
                {
                    await Task.Delay(SimulatedDelay);
                    byte[] response = GenerateRandomResponse(data);
                    SimulateReceive(response);
                });
                break;

            case SimulationMode.ModbusSlave:
                _ = Task.Run(async () =>
                {
                    await Task.Delay(SimulatedDelay);
                    byte[]? response = SimulateModbusResponse(data);
                    if (response != null) SimulateReceive(response);
                });
                break;
        }

        return true;
    }

    public bool SendHex(string hex)
    {
        try
        {
            byte[] data = ByteHelper.FromHexString(hex);
            return Send(data);
        }
        catch (Exception ex)
        {
            _logger.Error($"[模拟] 发送十六进制数据失败: {ex.Message}", nameof(SerialPortSimulator));
            return false;
        }
    }

    public bool SendAscii(string text)
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(text);
        return Send(data);
    }

    public static string[] GetAvailablePorts()
    {
        return ["COM1 (模拟)", "COM2 (模拟)", "COM3 (模拟)", "COM4 (模拟)"];
    }

    private void SimulateReceive(byte[] data)
    {
        if (!IsConnected) return;

        BytesReceived += data.Length;
        DataReceived?.Invoke(this, data);
        DataTransferred?.Invoke(this, new SerialDataEventArgs(data, false));
        _logger.Debug($"[模拟] 接收 {data.Length} 字节: {ByteHelper.ToHexString(data)}", nameof(SerialPortSimulator));
    }

    private byte[] GenerateRandomResponse(byte[] request)
    {
        int length = _random.Next(1, 16);
        byte[] response = new byte[length];
        _random.NextBytes(response);
        return response;
    }

    private byte[]? SimulateModbusResponse(byte[] request)
    {
        if (request.Length < 4) return null;

        byte slaveAddr = request[0];
        byte functionCode = request[1];

        return functionCode switch
        {
            0x03 => BuildReadHoldingRegistersResponse(slaveAddr, request),
            0x06 => request, // 写单个寄存器原样返回
            _ => null
        };
    }

    private byte[] BuildReadHoldingRegistersResponse(byte slaveAddr, byte[] request)
    {
        ushort quantity = (ushort)((request[4] << 8) | request[5]);
        quantity = Math.Min(quantity, (ushort)10);

        byte byteCount = (byte)(quantity * 2);
        byte[] response = new byte[3 + byteCount + 2];
        response[0] = slaveAddr;
        response[1] = 0x03;
        response[2] = byteCount;

        for (int i = 0; i < quantity; i++)
        {
            ushort value = (ushort)_random.Next(0, 1000);
            response[3 + i * 2] = (byte)(value >> 8);
            response[4 + i * 2] = (byte)value;
        }

        byte[] crc = CrcHelper.Crc16ModbusBytes(response, 0, response.Length - 2);
        response[^2] = crc[0];
        response[^1] = crc[1];
        return response;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Close();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// 模拟模式
/// </summary>
public enum SimulationMode
{
    /// <summary>回环模式 - 自发自收</summary>
    Loopback,
    /// <summary>随机响应 - 返回随机数据</summary>
    RandomResponse,
    /// <summary>Modbus 从站模拟 - 按 Modbus 协议返回响应</summary>
    ModbusSlave
}
