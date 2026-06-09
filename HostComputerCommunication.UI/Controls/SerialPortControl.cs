using System.IO.Ports;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.SerialPort;

namespace HostComputerCommunication.UI.Controls;

/// <summary>
/// 串口调试助手控件
/// 提供串口参数配置、数据收发（Hex/ASCII）、收发日志显示、模拟模式等功能
/// </summary>
public partial class SerialPortControl : UserControl
{
    /// <summary>真实串口管理器</summary>
    private SerialPortManager? _serialPort;

    /// <summary>串口模拟器（模拟模式时使用）</summary>
    private SerialPortSimulator? _simulator;

    /// <summary>日志记录器</summary>
    private readonly Logger _logger = new();

    /// <summary>当前是否为 Hex 模式（false 为 ASCII 模式）</summary>
    private bool _isHexMode = true;

    /// <summary>是否启用模拟模式</summary>
    private bool _isSimulationMode = false;

    public SerialPortControl()
    {
        InitializeComponent();
        _logger.LogReceived += Logger_LogReceived;
        RefreshPorts();
    }

    #region 连接配置事件

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        RefreshPorts();
    }

    private void ChkSimulation_CheckedChanged(object? sender, EventArgs e)
    {
        _isSimulationMode = chkSimulation.Checked;
        RefreshPorts();
    }

    private void BtnConnect_Click(object? sender, EventArgs e)
    {
        if (IsConnected)
            Disconnect();
        else
            Connect();
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

    #region 日志区事件

    private void BtnClearLog_Click(object? sender, EventArgs e)
    {
        rtbLog.Clear();
    }

    #endregion

    #region 定时器事件

    private void StatsTimer_Tick(object? sender, EventArgs e)
    {
        UpdateStats();
    }

    #endregion

    #region 串口事件回调

    private void OnDataTransferred(object? sender, SerialDataEventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnDataTransferred(sender, e));
            return;
        }

        string timestamp = chkShowTimestamp.Checked ? $"[{e.Timestamp:HH:mm:ss.fff}] " : "";
        Color color = e.IsSent ? Color.Cyan : Color.LimeGreen;
        string direction = e.IsSent ? "TX" : "RX";
        string data = _isHexMode ? e.HexString : e.AsciiString;

        AppendReceiveText(color, $"{timestamp}{direction} >> {data}\n");
    }

    private void OnConnectionStateChanged(object? sender, bool connected)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnConnectionStateChanged(sender, connected));
            return;
        }

        if (connected)
        {
            btnConnect.Text = "关闭串口";
            btnConnect.BackColor = Color.FromArgb(200, 50, 50);
            lblStatus.Text = "已连接";
            lblStatus.ForeColor = Color.Green;
            SetConfigEnabled(false);
        }
        else
        {
            btnConnect.Text = "打开串口";
            btnConnect.BackColor = Color.FromArgb(0, 120, 215);
            lblStatus.Text = "已断开";
            lblStatus.ForeColor = Color.Red;
            SetConfigEnabled(true);
        }
    }

    #endregion

    #region 日志回调

    private void Logger_LogReceived(object? sender, LogEventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(() => Logger_LogReceived(sender, e));
            return;
        }

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
        rtbLog.AppendText($"[{e.Timestamp:HH:mm:ss}] [{e.Level}] {e.Message}\n");
        rtbLog.ScrollToCaret();
    }

    #endregion

    #region 业务逻辑

    private bool IsConnected => _isSimulationMode
        ? _simulator?.IsConnected ?? false
        : _serialPort?.IsConnected ?? false;

    private void RefreshPorts()
    {
        cmbPort.Items.Clear();
        string[] ports = _isSimulationMode
            ? SerialPortSimulator.GetAvailablePorts()
            : SerialPortManager.GetAvailablePorts();
        cmbPort.Items.AddRange(ports);
        if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
    }

    private void Connect()
    {
        if (cmbPort.SelectedIndex < 0)
        {
            MessageBox.Show("请选择串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var config = BuildSerialPortConfig();

        if (_isSimulationMode)
        {
            if (_simulator == null)
            {
                _simulator = new SerialPortSimulator(_logger);
                _simulator.DataTransferred += OnDataTransferred;
                _simulator.ConnectionStateChanged += OnConnectionStateChanged;
            }
            _simulator.Mode = SimulationMode.Loopback;
            _simulator.Open(config);
        }
        else
        {
            if (_serialPort == null)
            {
                _serialPort = new SerialPortManager(_logger);
                _serialPort.DataTransferred += OnDataTransferred;
                _serialPort.ConnectionStateChanged += OnConnectionStateChanged;
            }
            _serialPort.Open(config);
        }

        _statsTimer.Start();
    }

    private void Disconnect()
    {
        if (_isSimulationMode)
            _simulator?.Close();
        else
            _serialPort?.Close();

        _statsTimer.Stop();
    }

    private void SendData()
    {
        string text = txtSend.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        bool success = _isSimulationMode
            ? (_isHexMode ? _simulator!.SendHex(text) : _simulator!.SendAscii(text))
            : (_isHexMode ? _serialPort!.SendHex(text) : _serialPort!.SendAscii(text));

        if (!success)
            AppendReceiveText(Color.Red, $"[发送失败] {text}\n");
    }

    private SerialPortConfig BuildSerialPortConfig()
    {
        return new SerialPortConfig
        {
            PortName = cmbPort.SelectedItem!.ToString()!.Split(' ')[0],
            BaudRate = int.Parse(cmbBaudRate.SelectedItem!.ToString()!),
            DataBits = int.Parse(cmbDataBits.SelectedItem!.ToString()!),
            Parity = cmbParity.SelectedIndex switch
            {
                1 => Parity.Odd,
                2 => Parity.Even,
                3 => Parity.Mark,
                4 => Parity.Space,
                _ => Parity.None
            },
            StopBits = cmbStopBits.SelectedIndex switch
            {
                1 => StopBits.OnePointFive,
                2 => StopBits.Two,
                _ => StopBits.One
            }
        };
    }

    private void SetConfigEnabled(bool enabled)
    {
        cmbPort.Enabled = enabled;
        cmbBaudRate.Enabled = enabled;
        cmbDataBits.Enabled = enabled;
        cmbParity.Enabled = enabled;
        cmbStopBits.Enabled = enabled;
        chkSimulation.Enabled = enabled;
        btnRefresh.Enabled = enabled;
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

    private void UpdateStats()
    {
        long sent = _isSimulationMode ? (_simulator?.BytesSent ?? 0) : (_serialPort?.BytesSent ?? 0);
        long received = _isSimulationMode ? (_simulator?.BytesReceived ?? 0) : (_serialPort?.BytesReceived ?? 0);
        lblStats.Text = $"收: {received} 字节 | 发: {sent} 字节";
    }

    private void ExportLog()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            FileName = $"SerialLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(dialog.FileName, rtbReceive.Text);
            MessageBox.Show("日志已导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    #endregion

}
