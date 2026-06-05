using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;

namespace HostComputerCommunication.TcpSocket;

/// <summary>
/// TCP 服务端管理器
/// </summary>
public class TcpServerManager : IDisposable
{
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private readonly Logger _logger;
    private readonly TcpConfig _config;
    private bool _disposed;

    private readonly ConcurrentDictionary<string, TcpClient> _clients = new();

    public event EventHandler<string>? ClientConnected;
    public event EventHandler<string>? ClientDisconnected;
    public event EventHandler<(string ClientId, byte[] Data)>? DataReceived;

    public bool IsRunning { get; private set; }
    public int ClientCount => _clients.Count;
    public IEnumerable<string> ConnectedClients => _clients.Keys;

    public TcpServerManager(Logger logger, TcpConfig config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// 启动服务端
    /// </summary>
    public async Task StartAsync()
    {
        if (IsRunning) return;

        try
        {
            _listener = new TcpListener(IPAddress.Any, _config.Port);
            _cts = new CancellationTokenSource();
            _listener.Start();

            IsRunning = true;
            _logger.Info($"TCP 服务端已启动，监听端口 {_config.Port}", nameof(TcpServerManager));

            _ = AcceptClientsAsync(_cts.Token);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.Error($"启动服务端失败: {ex.Message}", nameof(TcpServerManager));
            IsRunning = false;
        }
    }

    /// <summary>
    /// 停止服务端
    /// </summary>
    public void Stop()
    {
        try
        {
            _cts?.Cancel();
            _listener?.Stop();

            foreach (var (id, client) in _clients)
            {
                client.Dispose();
            }
            _clients.Clear();

            IsRunning = false;
            _logger.Info("TCP 服务端已停止", nameof(TcpServerManager));
        }
        catch (Exception ex)
        {
            _logger.Error($"停止服务端异常: {ex.Message}", nameof(TcpServerManager));
        }
    }

    /// <summary>
    /// 向指定客户端发送数据
    /// </summary>
    public async Task<bool> SendAsync(string clientId, byte[] data)
    {
        if (!_clients.TryGetValue(clientId, out var client))
        {
            _logger.Warning($"客户端 {clientId} 不存在", nameof(TcpServerManager));
            return false;
        }

        try
        {
            var stream = client.GetStream();
            await stream.WriteAsync(data);
            _logger.Debug($"向 {clientId} 发送 {data.Length} 字节", nameof(TcpServerManager));
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"发送失败: {ex.Message}", nameof(TcpServerManager));
            RemoveClient(clientId);
            return false;
        }
    }

    /// <summary>
    /// 向所有客户端广播
    /// </summary>
    public async Task BroadcastAsync(byte[] data)
    {
        foreach (var clientId in _clients.Keys)
        {
            await SendAsync(clientId, data);
        }
    }

    private async Task AcceptClientsAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested && _listener != null)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync(ct);
                string clientId = $"{((IPEndPoint)tcpClient.Client.RemoteEndPoint!).Address}:{((IPEndPoint)tcpClient.Client.RemoteEndPoint!).Port}";

                _clients[clientId] = tcpClient;
                _logger.Info($"客户端已连接: {clientId}", nameof(TcpServerManager));
                ClientConnected?.Invoke(this, clientId);

                _ = Task.Run(() => HandleClientAsync(clientId, tcpClient, ct));
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            if (IsRunning)
                _logger.Error($"接受连接异常: {ex.Message}", nameof(TcpServerManager));
        }
    }

    private async Task HandleClientAsync(string clientId, TcpClient client, CancellationToken ct)
    {
        byte[] buffer = new byte[4096];
        try
        {
            var stream = client.GetStream();
            while (!ct.IsCancellationRequested && client.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, ct);
                if (bytesRead == 0) break;

                byte[] data = new byte[bytesRead];
                Array.Copy(buffer, data, bytesRead);

                _logger.Debug($"从 {clientId} 接收 {bytesRead} 字节", nameof(TcpServerManager));
                DataReceived?.Invoke(this, (clientId, data));
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.Debug($"客户端 {clientId} 连接断开: {ex.Message}", nameof(TcpServerManager));
        }
        finally
        {
            RemoveClient(clientId);
        }
    }

    private void RemoveClient(string clientId)
    {
        if (_clients.TryRemove(clientId, out var client))
        {
            client.Dispose();
            _logger.Info($"客户端已断开: {clientId}", nameof(TcpServerManager));
            ClientDisconnected?.Invoke(this, clientId);
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Stop();
            _cts?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
