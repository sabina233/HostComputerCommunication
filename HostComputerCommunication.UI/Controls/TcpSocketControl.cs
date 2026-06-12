using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.TcpSocket;

namespace HostComputerCommunication.UI.Controls;

/// <summary>
/// TCP 通信工具控件
/// 支持客户端和服务端两种模式
/// 客户端：连接远程服务器，支持心跳和自动重连
/// 服务端：监听端口，支持多客户端管理和广播
/// </summary>
public partial class TcpSocketControl : UserControl
{
    /// <summary>TCP 客户端管理器</summary>
    private TcpClientManager? _tcpClient;

    /// <summary>TCP 服务端管理器</summary>
    private TcpServerManager? _tcpServer;

    /// <summary>日志记录器</summary>
    private readonly Logger _logger = new();

    /// <summary>当前是否为 Hex 模式</summary>
    private bool _isHexMode = true;

    public TcpSocketControl()
    {
        InitializeComponent();
        _logger.LogReceived += Logger_LogReceived;
        UpdateModeUI();
    }

    #region 连接配置事件

    private void RbClient_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateModeUI();
    }

    private async void BtnConnect_Click(object? sender, EventArgs e)
    {
        try
        {
            if (IsConnected)
                Disconnect();
            else
                await ConnectAsync();
        }
        catch (Exception ex)
        {
            _logger.Error($"BtnConnect_Click 严重错误: {ex}", nameof(TcpSocketControl));
        }
    }

    #endregion

    #region 发送区事件

    private void RbHex_CheckedChanged(object? sender, EventArgs e)
    {
        _isHexMode = rbHex.Checked;
    }

    private void TxtSend_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && e.Control)
        {
            SendData();
            e.SuppressKeyPress = true;
        }
    }

    private void BtnSend_Click(object? sender, EventArgs e)
    {
        SendData();
    }

    private void BtnClearSend_Click(object? sender, EventArgs e)
    {
        txtSend.Clear();
    }

    #endregion

    #region 接收区事件

    private void BtnClearReceive_Click(object? sender, EventArgs e)
    {
        rtbReceive.Clear();
    }

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        ExportLog();
    }

    #endregion

    #region 日志事件

    private void BtnClearLog_Click(object? sender, EventArgs e)
    {
        rtbLog.Clear();
    }

    private void Logger_LogReceived(object? sender, LogEventArgs e)
    {
        if (InvokeRequired) { Invoke(() => Logger_LogReceived(sender, e)); return; }

        Color color = e.Level switch
        {
            LogLevel.Debug => Color.Gray,
            LogLevel.Info => Color.White,
            LogLevel.Warning => Color.Yellow,
            LogLevel.Error => Color.Red,
            _ => Color.White
        };

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.SelectionLength = 0;
        rtbLog.SelectionColor = color;
        rtbLog.AppendText($"[{e.Timestamp:HH:mm:ss}] {e.Message}\n");
        rtbLog.ScrollToCaret();
    }

    #endregion

    #region TCP 事件回调

    private void OnClientDataReceived(object? sender, byte[] data)
    {
        if (InvokeRequired) { Invoke(() => OnClientDataReceived(sender, data)); return; }
        DisplayReceivedData(null, data);
    }

    private void OnServerDataReceived(object? sender, (string ClientId, byte[] Data) e)
    {
        if (InvokeRequired) { Invoke(() => OnServerDataReceived(sender, e)); return; }
        DisplayReceivedData(e.ClientId, e.Data);
    }

    private void OnClientConnectionStateChanged(object? sender, bool connected)
    {
        if (InvokeRequired) { Invoke(() => OnClientConnectionStateChanged(sender, connected)); return; }
        UpdateConnectionUI(connected);
    }

    private void OnClientConnected(object? sender, string clientId)
    {
        if (InvokeRequired) { Invoke(() => OnClientConnected(sender, clientId)); return; }
        lstClients.Items.Add(clientId);
        lblClientCount.Text = $"已连接: {_tcpServer?.ClientCount ?? 0}";
    }

    private void OnClientDisconnected(object? sender, string clientId)
    {
        if (InvokeRequired) { Invoke(() => OnClientDisconnected(sender, clientId)); return; }
        lstClients.Items.Remove(clientId);
        lblClientCount.Text = $"已连接: {_tcpServer?.ClientCount ?? 0}";
    }

    #endregion

    #region UI 状态管理

    private bool IsClientMode => rbClient.Checked;

    private bool IsConnected => IsClientMode
        ? (_tcpClient?.IsConnected ?? false)
        : (_tcpServer?.IsRunning ?? false);

    private void UpdateModeUI()
    {
        bool isClient = rbClient.Checked;
        lblHost.Text = isClient ? "主机:" : "监听:";
        txtHost.Enabled = isClient;
        chkAutoReconnect.Visible = isClient;
        grpClients.Visible = !isClient;
    }

    private void UpdateConnectionUI(bool connected)
    {
        if (InvokeRequired) { Invoke(() => UpdateConnectionUI(connected)); return; }

        if (connected)
        {
            btnConnect.Text = IsClientMode ? "断开" : "停止";
            btnConnect.BackColor = Color.FromArgb(200, 50, 50);
            lblStatus.Text = IsClientMode ? "已连接" : "监听中";
            lblStatus.ForeColor = Color.Green;
        }
        else
        {
            btnConnect.Text = IsClientMode ? "连接" : "启动";
            btnConnect.BackColor = Color.FromArgb(0, 120, 215);
            lblStatus.Text = "未连接";
            lblStatus.ForeColor = Color.Red;
        }
        SetConfigEnabled(!connected);
    }

    private void SetConfigEnabled(bool enabled)
    {
        rbClient.Enabled = enabled;
        rbServer.Enabled = enabled;
        txtHost.Enabled = enabled && IsClientMode;
        nudPort.Enabled = enabled;
        chkAutoReconnect.Enabled = enabled;
    }

    #endregion

    #region 连接管理

    private async Task ConnectAsync()
    {
        if (IsClientMode)
        {
            var config = new TcpConfig
            {
                Host = txtHost.Text.Trim(),
                Port = (int)nudPort.Value,
                AutoReconnect = chkAutoReconnect.Checked
            };
            _tcpClient?.Dispose();
            _tcpClient = new TcpClientManager(_logger, config);
            _tcpClient.DataReceived += OnClientDataReceived;
            _tcpClient.ConnectionStateChanged += OnClientConnectionStateChanged;
            await _tcpClient.ConnectAsync();
        }
        else
        {
            var config = new TcpConfig { Port = (int)nudPort.Value };
            _tcpServer?.Dispose();
            _tcpServer = new TcpServerManager(_logger, config);
            _tcpServer.DataReceived += OnServerDataReceived;
            _tcpServer.ClientConnected += OnClientConnected;
            _tcpServer.ClientDisconnected += OnClientDisconnected;
            await _tcpServer.StartAsync();
            UpdateConnectionUI(true);
        }
    }

    private void Disconnect()
    {
        if (IsClientMode)
        {
            _tcpClient?.Dispose();
            _tcpClient = null;
        }
        else
        {
            _tcpServer?.Stop();
            UpdateConnectionUI(false);
        }
    }

    #endregion

    #region 数据收发

    private async void SendData()
    {
        try
        {
            string text = txtSend.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            byte[] data;
            try
            {
                data = _isHexMode ? ByteHelper.FromHexString(text) : System.Text.Encoding.ASCII.GetBytes(text);
            }
            catch (Exception ex)
            {
                AppendReceiveText(Color.Red, $"[格式错误] {ex.Message}\n");
                return;
            }

            bool success;
            if (IsClientMode)
            {
                success = _tcpClient != null && await _tcpClient.SendAsync(data);
            }
            else
            {
                if (lstClients.SelectedItem is string clientId)
                    success = _tcpServer != null && await _tcpServer.SendAsync(clientId, data);
                else
                {
                    AppendReceiveText(Color.Yellow, "[提示] 请先选择一个客户端\n");
                    return;
                }
            }

            if (success)
            {
                string display = _isHexMode ? ByteHelper.ToHexString(data) : System.Text.Encoding.ASCII.GetString(data);
                AppendReceiveText(Color.Cyan, $"[{DateTime.Now:HH:mm:ss.fff}] TX >> {display}\n");
            }
            else
            {
                AppendReceiveText(Color.Red, "[发送失败]\n");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SendData 严重错误: {ex}", nameof(TcpSocketControl));
        }
    }

    private void DisplayReceivedData(string? clientId, byte[] data)
    {
        string timestamp = $"[{DateTime.Now:HH:mm:ss.fff}] ";
        string prefix = clientId != null ? $"{clientId} " : "";
        string display = _isHexMode ? ByteHelper.ToHexString(data) : System.Text.Encoding.ASCII.GetString(data);
        AppendReceiveText(Color.LimeGreen, $"{timestamp}{prefix}RX >> {display}\n");
    }

    private void AppendReceiveText(Color color, string text)
    {
        rtbReceive.SelectionStart = rtbReceive.TextLength;
        rtbReceive.SelectionLength = 0;
        rtbReceive.SelectionColor = color;
        rtbReceive.AppendText(text);

        if (chkAutoScroll.Checked)
        {
            rtbReceive.SelectionStart = rtbReceive.TextLength;
            rtbReceive.ScrollToCaret();
        }
    }

    private void ExportLog()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            FileName = $"TcpLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(dialog.FileName, rtbReceive.Text);
        }
    }

    #endregion
}
