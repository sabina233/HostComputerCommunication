namespace HostComputerCommunication.SerialPort;

/// <summary>
/// 串口通信接口，支持真实串口和模拟器
/// </summary>
public interface ISerialPort : IDisposable
{
    event EventHandler<byte[]>? DataReceived;
    event EventHandler<SerialDataEventArgs>? DataTransferred;
    event EventHandler<bool>? ConnectionStateChanged;

    bool IsConnected { get; }
    long BytesSent { get; }
    long BytesReceived { get; }

    bool Send(byte[] data);
    bool SendHex(string hex);
    bool SendAscii(string text);
    void Close();
}
