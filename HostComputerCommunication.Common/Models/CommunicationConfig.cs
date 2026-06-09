namespace HostComputerCommunication.Common.Models;

/// <summary>
/// 串口通信配置
/// 包含串口通信所需的全部参数
/// </summary>
public class SerialPortConfig
{
    /// <summary>串口名称，如 COM1、COM3</summary>
    public string PortName { get; set; } = "COM1";

    /// <summary>波特率，常用值: 9600, 19200, 38400, 57600, 115200</summary>
    public int BaudRate { get; set; } = 9600;

    /// <summary>数据位，通常为 7 或 8</summary>
    public int DataBits { get; set; } = 8;

    /// <summary>校验位: None(无), Odd(奇), Even(偶), Mark(标记), Space(空)</summary>
    public System.IO.Ports.Parity Parity { get; set; } = System.IO.Ports.Parity.None;

    /// <summary>停止位: One(1), OnePointFive(1.5), Two(2)</summary>
    public System.IO.Ports.StopBits StopBits { get; set; } = System.IO.Ports.StopBits.One;

    /// <summary>读取超时时间（毫秒）</summary>
    public int ReadTimeout { get; set; } = 1000;

    /// <summary>写入超时时间（毫秒）</summary>
    public int WriteTimeout { get; set; } = 1000;
}

/// <summary>
/// TCP 通信配置
/// </summary>
public class TcpConfig
{
    /// <summary>服务器 IP 地址</summary>
    public string Host { get; set; } = "127.0.0.1";

    /// <summary>服务器端口号</summary>
    public int Port { get; set; } = 502;

    /// <summary>连接超时时间（毫秒）</summary>
    public int ConnectTimeout { get; set; } = 3000;

    /// <summary>读取超时时间（毫秒）</summary>
    public int ReadTimeout { get; set; } = 3000;

    /// <summary>心跳检测间隔（毫秒），0 表示不启用心跳</summary>
    public int HeartbeatInterval { get; set; } = 5000;

    /// <summary>是否启用断线自动重连</summary>
    public bool AutoReconnect { get; set; } = true;

    /// <summary>重连间隔时间（毫秒）</summary>
    public int ReconnectInterval { get; set; } = 3000;
}

/// <summary>
/// Modbus 通信配置
/// </summary>
public class ModbusConfig
{
    /// <summary>从站地址（站号），范围 1-247</summary>
    public byte SlaveAddress { get; set; } = 1;

    /// <summary>通信超时时间（毫秒）</summary>
    public int Timeout { get; set; } = 1000;

    /// <summary>通信失败重试次数</summary>
    public int Retries { get; set; } = 3;
}

/// <summary>
/// OPC UA 通信配置
/// </summary>
public class OpcUaConfig
{
    /// <summary>OPC UA 服务器端点 URL，如 opc.tcp://localhost:4840</summary>
    public string EndpointUrl { get; set; } = "opc.tcp://localhost:4840";

    /// <summary>用户名（可选，匿名连接时为空）</summary>
    public string? Username { get; set; }

    /// <summary>密码（可选）</summary>
    public string? Password { get; set; }

    /// <summary>是否自动接受不受信任的证书</summary>
    public bool AutoAccept { get; set; } = true;

    /// <summary>会话超时时间（毫秒）</summary>
    public int SessionTimeout { get; set; } = 60000;
}
