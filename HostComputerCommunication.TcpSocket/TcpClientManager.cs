using System.Net;
using System.Net.Sockets;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;

namespace HostComputerCommunication.TcpSocket;

/// <summary>
/// TCP 客户端管理器
/// </summary>
public class TcpClientManager : IDisposable
{
    private System.Net.Sockets.TcpClient? _client;
    private NetworkStream? _stream;
    private CancellationTokenSource? _cts;
    private Task? _receiveTask;
    private Task? _heartbeatTask;
    private readonly Logger _logger;
    private readonly TcpConfig _config;
    private bool _disposed;
    private bool _isConnecting;

    public event EventHandler<byte[]>? DataReceived;
    public event EventHandler<bool>? ConnectionStateChanged;

    public bool IsConnected => _client?.Connected ?? false;
    public TcpConfig Config => _config;

    public TcpClientManager(Logger logger, TcpConfig config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// 连接到服务器
    /// </summary>
    public async Task<bool> ConnectAsync()
    {
        if (IsConnected || _isConnecting) return IsConnected;

        _isConnecting = true;
        try
        {
            _client = new System.Net.Sockets.TcpClient();
            _cts = new CancellationTokenSource();

            var connectTask = _client.ConnectAsync(_config.Host, _config.Port);
            if (await Task.WhenAny(connectTask, Task.Delay(_config.ConnectTimeout)) != connectTask)
            {
                _logger.Error($"连接超时: {_config.Host}:{_config.Port}", nameof(TcpClientManager));
                _client.Dispose();
                _client = null;
                return false;
            }

            await connectTask;
            _stream = _client.GetStream();

            _logger.Info($"已连接到 {_config.Host}:{_config.Port}", nameof(TcpClientManager));
            ConnectionStateChanged?.Invoke(this, true);

            // 启动接收任务
            _receiveTask = Task.Run(() => ReceiveLoopAsync(_cts.Token));

            // 启动心跳任务
            if (_config.HeartbeatInterval > 0)
            {
                _heartbeatTask = Task.Run(() => HeartbeatLoopAsync(_cts.Token));
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"连接失败: {ex.Message}", nameof(TcpClientManager));
            return false;
        }
        finally
        {
            _isConnecting = false;
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        try
        {
            _cts?.Cancel();
            _stream?.Dispose();
            _client?.Dispose();
            _client = null;
            _stream = null;

            _logger.Info("已断开连接", nameof(TcpClientManager));
            ConnectionStateChanged?.Invoke(this, false);
        }
        catch (Exception ex)
        {
            _logger.Error($"断开连接异常: {ex.Message}", nameof(TcpClientManager));
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public async Task<bool> SendAsync(byte[] data)
    {
        if (!IsConnected || _stream == null)
        {
            _logger.Warning("未连接，无法发送数据", nameof(TcpClientManager));
            return false;
        }

        try
        {
            await _stream.WriteAsync(data);
            _logger.Debug($"发送 {data.Length} 字节: {ByteHelper.ToHexString(data)}", nameof(TcpClientManager));
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"发送失败: {ex.Message}", nameof(TcpClientManager));
            return false;
        }
    }

    /// <summary>
    /// 接收循环
    /// </summary>
    private async Task ReceiveLoopAsync(CancellationToken ct)
    {
        byte[] buffer = new byte[4096];
        try
        {
            while (!ct.IsCancellationRequested && _stream != null)
            {
                int bytesRead = await _stream.ReadAsync(buffer, ct);
                if (bytesRead == 0)
                {
                    _logger.Info("服务器断开连接", nameof(TcpClientManager));
                    break;
                }

                byte[] data = new byte[bytesRead];
                Array.Copy(buffer, data, bytesRead);

                _logger.Debug($"接收 {bytesRead} 字节: {ByteHelper.ToHexString(data)}", nameof(TcpClientManager));
                DataReceived?.Invoke(this, data);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.Error($"接收异常: {ex.Message}", nameof(TcpClientManager));
        }
        finally
        {
            if (_config.AutoReconnect && !ct.IsCancellationRequested)
            {
                _ = Task.Run(() => ReconnectAsync(ct));
            }
            else
            {
                ConnectionStateChanged?.Invoke(this, false);
            }
        }
    }

    /// <summary>
    /// 心跳循环
    /// </summary>
    private async Task HeartbeatLoopAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(_config.HeartbeatInterval, ct);
                if (IsConnected)
                {
                    // 发送空数据作为心跳（可根据协议自定义）
                    _logger.Debug("心跳检测", nameof(TcpClientManager));
                }
            }
        }
        catch (OperationCanceledException) { }
    }

    /// <summary>
    /// 自动重连
    /// </summary>
    private async Task ReconnectAsync(CancellationToken ct)
    {
        _logger.Info($"将在 {_config.ReconnectInterval / 1000} 秒后尝试重连...", nameof(TcpClientManager));
        await Task.Delay(_config.ReconnectInterval, ct);

        if (!ct.IsCancellationRequested)
        {
            _logger.Info("尝试重连...", nameof(TcpClientManager));
            await ConnectAsync();
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Disconnect();
            _cts?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
