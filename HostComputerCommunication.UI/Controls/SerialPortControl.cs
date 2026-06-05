using System.IO.Ports;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.SerialPort;

namespace HostComputerCommunication.UI.Controls;

public partial class SerialPortControl : UserControl
{
    private SerialPortManager? _serialPort;
    private SerialPortSimulator? _simulator;
    private readonly Logger _logger = new();
    private bool _isHexMode = true;
    private bool _isSimulationMode = false;

    public SerialPortControl()
    {
        InitializeComponent();
        SetupEvents();
        RefreshPorts();
    }

    private void SetupEvents()
    {
        _logger.LogReceived += OnLogReceived;
        btnRefresh.Click += (s, e) => RefreshPorts();
        btnConnect.Click += (s, e) => ToggleConnection();
        btnSend.Click += (s, e) => SendData();
        btnClearSend.Click += (s, e) => txtSend.Clear();
        btnClearReceive.Click += (s, e) => rtbReceive.Clear();
        btnClearLog.Click += (s, e) => rtbLog.Clear();
        btnExport.Click += (s, e) => ExportLog();
        rbHex.CheckedChanged += (s, e) => _isHexMode = rbHex.Checked;
        txtSend.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter && e.Control) { SendData(); e.SuppressKeyPress = true; } };
        chkSimulation.CheckedChanged += (s, e) =>
        {
            _isSimulationMode = chkSimulation.Checked;
            RefreshPorts();
        };
        _statsTimer.Tick += (s, e) => UpdateStats();
    }

    private void RefreshPorts()
    {
        cmbPort.Items.Clear();
        string[] ports = _isSimulationMode
            ? SerialPortSimulator.GetAvailablePorts()
            : SerialPortManager.GetAvailablePorts();
        cmbPort.Items.AddRange(ports);
        if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
    }

    private bool IsConnected => _isSimulationMode
        ? _simulator?.IsConnected ?? false
        : _serialPort?.IsConnected ?? false;

    private void ToggleConnection()
    {
        if (IsConnected)
            Disconnect();
        else
            Connect();
    }

    private void Connect()
    {
        if (cmbPort.SelectedIndex < 0)
        {
            MessageBox.Show("请选择串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var config = new SerialPortConfig
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

    private void OnLogReceived(object? sender, LogEventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnLogReceived(sender, e));
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
}
