using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.OpcUa;
using Opc.Ua;

namespace HostComputerCommunication.UI.Controls;

public partial class OpcUaControl : UserControl
{
    private OpcUaClient? _opcUaClient;
    private readonly Logger _logger = new();
    private readonly OpcUaConfig _config = new();

    public OpcUaControl()
    {
        InitializeComponent();
        SetupEvents();
        SetupDataGridView();
    }

    private void SetupEvents()
    {
        _logger.LogReceived += OnLogReceived;
        btnConnect.Click += async (s, e) => await ConnectAsync();
        btnDisconnect.Click += async (s, e) => await DisconnectAsync();
        btnBrowse.Click += async (s, e) => await BrowseRootAsync();
        btnRead.Click += async (s, e) => await ReadNodeAsync();
        btnWrite.Click += async (s, e) => await WriteNodeAsync();
        btnSubscribe.Click += async (s, e) => await SubscribeNodeAsync();
        btnClearLog.Click += (s, e) => rtbLog.Clear();
        tvNodes.AfterSelect += OnNodeSelected;
    }

    private void SetupDataGridView()
    {
        dgvData.Columns.Add("NodeId", "节点ID");
        dgvData.Columns.Add("DisplayName", "名称");
        dgvData.Columns.Add("Value", "当前值");
        dgvData.Columns.Add("Timestamp", "时间戳");
    }

    private async Task ConnectAsync()
    {
        _config.EndpointUrl = txtEndpoint.Text.Trim();
        _config.Username = txtUsername.Text.Trim();
        _config.Password = txtPassword.Text.Trim();

        _opcUaClient?.Dispose();
        _opcUaClient = new OpcUaClient(_logger, _config);
        _opcUaClient.DataValueChanged += OnDataValueChanged;

        UpdateUI(false);
        _logger.Info("正在连接 OPC UA 服务器...", nameof(OpcUaControl));

        bool success = await _opcUaClient.ConnectAsync();
        UpdateUI(success);
    }

    private async Task DisconnectAsync()
    {
        if (_opcUaClient != null)
        {
            await _opcUaClient.DisconnectAsync();
        }
        UpdateUI(false);
    }

    private async Task BrowseRootAsync()
    {
        if (_opcUaClient == null || !_opcUaClient.IsConnected)
        {
            MessageBox.Show("请先连接到 OPC UA 服务器", "提示");
            return;
        }

        tvNodes.Nodes.Clear();
        try
        {
            var results = await _opcUaClient.BrowseAsync(ObjectIds.ObjectsFolder);
            int count = 0;
            foreach (var result in results)
            {
                foreach (var reference in result.References)
                {
                    var node = new TreeNode(reference.DisplayName.Text)
                    {
                        Tag = reference.NodeId
                    };
                    tvNodes.Nodes.Add(node);
                    count++;
                }
            }
            _logger.Info($"浏览完成，找到 {count} 个节点", nameof(OpcUaControl));
        }
        catch (Exception ex)
        {
            _logger.Error($"浏览失败: {ex.Message}", nameof(OpcUaControl));
        }
    }

    private async Task ReadNodeAsync()
    {
        if (_opcUaClient == null || !_opcUaClient.IsConnected)
        {
            MessageBox.Show("请先连接到 OPC UA 服务器", "提示");
            return;
        }

        try
        {
            var nodeId = new NodeId(txtNodeId.Text.Trim());
            var dataValue = await _opcUaClient.ReadAsync(nodeId);
            txtValue.Text = dataValue.Value?.ToString() ?? "(null)";
            _logger.Info($"读取 {txtNodeId.Text} = {dataValue.Value}", nameof(OpcUaControl));
        }
        catch (Exception ex)
        {
            _logger.Error($"读取失败: {ex.Message}", nameof(OpcUaControl));
        }
    }

    private async Task WriteNodeAsync()
    {
        if (_opcUaClient == null || !_opcUaClient.IsConnected)
        {
            MessageBox.Show("请先连接到 OPC UA 服务器", "提示");
            return;
        }

        try
        {
            var nodeId = new NodeId(txtNodeId.Text.Trim());
            bool success = await _opcUaClient.WriteAsync(nodeId, txtValue.Text);
            if (success)
                _logger.Info($"写入 {txtNodeId.Text} = {txtValue.Text} 成功", nameof(OpcUaControl));
            else
                _logger.Warning($"写入 {txtNodeId.Text} 失败", nameof(OpcUaControl));
        }
        catch (Exception ex)
        {
            _logger.Error($"写入失败: {ex.Message}", nameof(OpcUaControl));
        }
    }

    private async Task SubscribeNodeAsync()
    {
        if (_opcUaClient == null || !_opcUaClient.IsConnected)
        {
            MessageBox.Show("请先连接到 OPC UA 服务器", "提示");
            return;
        }

        try
        {
            var subscription = await _opcUaClient.CreateSubscriptionAsync(1000);
            var nodeId = new NodeId(txtNodeId.Text.Trim());
            await _opcUaClient.AddMonitoredItemAsync(subscription, nodeId, txtNodeId.Text.Trim());
            _logger.Info($"已订阅节点 {txtNodeId.Text}", nameof(OpcUaControl));

            dgvData.Rows.Add(txtNodeId.Text.Trim(), txtNodeId.Text.Trim(), "等待数据...", DateTime.Now.ToString("HH:mm:ss"));
        }
        catch (Exception ex)
        {
            _logger.Error($"订阅失败: {ex.Message}", nameof(OpcUaControl));
        }
    }

    private void OnNodeSelected(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is NodeId nodeId)
        {
            txtNodeId.Text = nodeId.ToString();
        }
    }

    private void OnDataValueChanged(object? sender, DataValueChangedEventArgs e)
    {
        if (InvokeRequired) { Invoke(() => OnDataValueChanged(sender, e)); return; }

        foreach (DataGridViewRow row in dgvData.Rows)
        {
            if (row.Cells["DisplayName"].Value?.ToString() == e.DisplayName)
            {
                row.Cells["Value"].Value = e.DataValue.Value?.ToString();
                row.Cells["Timestamp"].Value = e.DataValue.SourceTimestamp.ToString("HH:mm:ss.fff");
                return;
            }
        }
    }

    private void UpdateUI(bool connected)
    {
        if (InvokeRequired) { Invoke(() => UpdateUI(connected)); return; }

        btnConnect.Enabled = !connected;
        btnDisconnect.Enabled = connected;
        txtEndpoint.Enabled = !connected;
        txtUsername.Enabled = !connected;
        txtPassword.Enabled = !connected;

        if (connected)
        {
            lblStatus.Text = "已连接";
            lblStatus.ForeColor = Color.Green;
        }
        else
        {
            lblStatus.Text = "未连接";
            lblStatus.ForeColor = Color.Red;
        }
    }

    private void OnLogReceived(object? sender, LogEventArgs e)
    {
        if (InvokeRequired) { Invoke(() => OnLogReceived(sender, e)); return; }

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
}
