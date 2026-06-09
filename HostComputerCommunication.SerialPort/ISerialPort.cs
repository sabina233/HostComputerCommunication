namespace HostComputerCommunication.SerialPort;

/// <summary>
/// 串口通信接口
/// 统一真实串口（SerialPortManager）和模拟器（SerialPortSimulator）的调用方式
/// </summary>
public interface ISerialPort : IDisposable
{
    /// <summary>数据接收事件（原始字节数组）</summary>
    event EventHandler<byte[]>? DataReceived;

    /// <summary>数据收发事件（格式化后的数据，含时间戳和方向标识）</summary>
    event EventHandler<SerialDataEventArgs>? DataTransferred;

    /// <summary>连接状态变更事件</summary>
    event EventHandler<bool>? ConnectionStateChanged;

    /// <summary>是否已连接</summary>
    bool IsConnected { get; }

    /// <summary>已发送字节数统计</summary>
    long BytesSent { get; }

    /// <summary>已接收字节数统计</summary>
    long BytesReceived { get; }

    /// <summary>发送原始字节数组</summary>
    /// <param name="data">待发送的数据</param>
    /// <returns>发送是否成功</returns>
    bool Send(byte[] data);

    /// <summary>发送十六进制字符串（自动转换为字节数组）</summary>
    /// <param name="hex">十六进制字符串，如 "01 03 00 00"</param>
    bool SendHex(string hex);

    /// <summary>发送 ASCII 文本（自动编码为字节数组）</summary>
    bool SendAscii(string text);

    /// <summary>关闭连接</summary>
    void Close();
}
