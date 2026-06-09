using System.IO.Ports;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;

namespace HostComputerCommunication.SerialPort;

/// <summary>
/// 串口数据事件参数
/// 包含收发数据的格式化信息，用于 UI 显示
/// </summary>
public class SerialDataEventArgs : EventArgs
{
    /// <summary>数据收发时间戳</summary>
    public DateTime Timestamp { get; }

    /// <summary>原始字节数组</summary>
    public byte[] Data { get; }

    /// <summary>十六进制格式字符串，如 "01 03 FF"</summary>
    public string HexString { get; }

    /// <summary>ASCII 格式字符串（不可显示字符替换为 ·）</summary>
    public string AsciiString { get; }

    /// <summary>是否为发送的数据（false 表示接收）</summary>
    public bool IsSent { get; }

    public SerialDataEventArgs(byte[] data, bool isSent)
    {
        Timestamp = DateTime.Now;
        Data = data;
        HexString = ByteHelper.ToHexString(data);
        AsciiString = System.Text.Encoding.ASCII.GetString(data)
            .Replace('\0', '·').Replace('\r', '·').Replace('\n', '·');
        IsSent = isSent;
    }
}

/// <summary>
/// 串口管理器
/// 封装 System.IO.Ports.SerialPort，提供简化的串口通信接口
/// 支持串口参数配置、数据收发、收发日志记录
/// </summary>
public class SerialPortManager : ISerialPort
{
    private System.IO.Ports.SerialPort? _serialPort;
    private readonly Logger _logger;
    private bool _disposed;

    public event EventHandler<byte[]>? DataReceived;
    public event EventHandler<SerialDataEventArgs>? DataTransferred;
    public event EventHandler<bool>? ConnectionStateChanged;

    public bool IsConnected => _serialPort?.IsOpen ?? false;
    public SerialPortConfig Config { get; private set; }
    public long BytesSent { get; private set; }
    public long BytesReceived { get; private set; }

    public SerialPortManager(Logger logger)
    {
        _logger = logger;
        Config = new SerialPortConfig();
    }

    public bool Open(SerialPortConfig config)
    {
        try
        {
            Close();
            Config = config;

            _serialPort = new System.IO.Ports.SerialPort
            {
                PortName = config.PortName,
                BaudRate = config.BaudRate,
                DataBits = config.DataBits,
                Parity = config.Parity,
                StopBits = config.StopBits,
                ReadTimeout = config.ReadTimeout,
                WriteTimeout = config.WriteTimeout
            };

            _serialPort.DataReceived += OnDataReceived;
            _serialPort.Open();

            BytesSent = 0;
            BytesReceived = 0;

            _logger.Info($"串口 {config.PortName} 已打开 ({config.BaudRate},{config.DataBits},{config.Parity},{config.StopBits})", nameof(SerialPortManager));
            ConnectionStateChanged?.Invoke(this, true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"打开串口失败: {ex.Message}", nameof(SerialPortManager));
            return false;
        }
    }

    public void Close()
    {
        if (_serialPort != null)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    _logger.Info($"串口 {_serialPort.PortName} 已关闭", nameof(SerialPortManager));
                }
                _serialPort.DataReceived -= OnDataReceived;
                _serialPort.Dispose();
                _serialPort = null;
                ConnectionStateChanged?.Invoke(this, false);
            }
            catch (Exception ex)
            {
                _logger.Error($"关闭串口失败: {ex.Message}", nameof(SerialPortManager));
            }
        }
    }

    public bool Send(byte[] data)
    {
        if (!IsConnected || _serialPort == null)
        {
            _logger.Warning("串口未连接，无法发送数据", nameof(SerialPortManager));
            return false;
        }

        try
        {
            _serialPort.Write(data, 0, data.Length);
            BytesSent += data.Length;
            DataTransferred?.Invoke(this, new SerialDataEventArgs(data, true));
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"发送数据失败: {ex.Message}", nameof(SerialPortManager));
            return false;
        }
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
            _logger.Error($"发送十六进制数据失败: {ex.Message}", nameof(SerialPortManager));
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
        return System.IO.Ports.SerialPort.GetPortNames();
    }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (_serialPort == null || !_serialPort.IsOpen) return;

        try
        {
            int bytesToRead = _serialPort.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            _serialPort.Read(buffer, 0, bytesToRead);

            BytesReceived += bytesToRead;
            DataReceived?.Invoke(this, buffer);
            DataTransferred?.Invoke(this, new SerialDataEventArgs(buffer, false));
        }
        catch (Exception ex)
        {
            _logger.Error($"接收数据失败: {ex.Message}", nameof(SerialPortManager));
        }
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
