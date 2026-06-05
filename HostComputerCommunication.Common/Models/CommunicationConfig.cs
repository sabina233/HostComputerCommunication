namespace HostComputerCommunication.Common.Models;

/// <summary>
/// 串口配置
/// </summary>
public class SerialPortConfig
{
    public string PortName { get; set; } = "COM1";
    public int BaudRate { get; set; } = 9600;
    public int DataBits { get; set; } = 8;
    public System.IO.Ports.Parity Parity { get; set; } = System.IO.Ports.Parity.None;
    public System.IO.Ports.StopBits StopBits { get; set; } = System.IO.Ports.StopBits.One;
    public int ReadTimeout { get; set; } = 1000;
    public int WriteTimeout { get; set; } = 1000;
}

/// <summary>
/// TCP 配置
/// </summary>
public class TcpConfig
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 502;
    public int ConnectTimeout { get; set; } = 3000;
    public int ReadTimeout { get; set; } = 3000;
    public int HeartbeatInterval { get; set; } = 5000;
    public bool AutoReconnect { get; set; } = true;
    public int ReconnectInterval { get; set; } = 3000;
}

/// <summary>
/// Modbus 配置
/// </summary>
public class ModbusConfig
{
    public byte SlaveAddress { get; set; } = 1;
    public int Timeout { get; set; } = 1000;
    public int Retries { get; set; } = 3;
}

/// <summary>
/// OPC UA 配置
/// </summary>
public class OpcUaConfig
{
    public string EndpointUrl { get; set; } = "opc.tcp://localhost:4840";
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool AutoAccept { get; set; } = true;
    public int SessionTimeout { get; set; } = 60000;
}
