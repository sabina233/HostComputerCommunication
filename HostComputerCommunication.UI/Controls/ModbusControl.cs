using System.IO.Ports;
using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using HostComputerCommunication.Modbus;
using HostComputerCommunication.Modbus.Protocol;
using HostComputerCommunication.SerialPort;
using HostComputerCommunication.TcpSocket;

namespace HostComputerCommunication.UI.Controls;

/// <summary>
/// Modbus 调试工具控件
/// 支持 Modbus RTU（串口）和 Modbus TCP（以太网）两种通信方式
/// 可读写寄存器和线圈，支持多种数据类型解析
/// </summary>
public partial class ModbusControl : UserControl
{
    /// <summary>Modbus RTU 客户端（串口通信）</summary>
    private ModbusRtuClient? _rtuClient;

    /// <summary>Modbus TCP 客户端（以太网通信）</summary>
    private ModbusTcpClient? _tcpClient;

    /// <summary>真实串口管理器</summary>
    private SerialPortManager? _serialPort;

    /// <summary>串口模拟器</summary>
    private SerialPortSimulator? _simulator;

    /// <summary>TCP 客户端管理器</summary>
    private TcpClientManager? _tcpManager;

    /// <summary>日志记录器</summary>
    private readonly Logger _logger = new();

    public ModbusControl()
    {
        InitializeComponent();
        SetupFunctionCodes();
        SetupDataGridView();
        _logger.LogReceived += Logger_LogReceived;
        RefreshPorts();
        UpdateModeUI();
        UpdateWriteUI();
    }

    #region 初始化

    private void SetupFunctionCodes()
    {
        cmbFunction.Items.AddRange(new object[]
        {
            "01 - 读线圈",
            "02 - 读离散输入",
            "03 - 读保持寄存器",
            "04 - 读输入寄存器",
            "05 - 写单个线圈",
            "06 - 写单个寄存器",
            "15 - 写多个线圈",
            "16 - 写多个寄存器"
        });
        cmbFunction.SelectedIndex = 2;
        cmbDataType.SelectedIndex = 0;
    }

    private void SetupDataGridView()
    {
        dgvRegisters.Columns.Add("Address", "地址");
        dgvRegisters.Columns.Add("RawValue", "原始值");
        dgvRegisters.Columns.Add("Value", "解析值");
    }

    #endregion

    #region 连接配置事件

    private void RbRtu_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateModeUI();
    }

    private void ChkSimulation_CheckedChanged(object? sender, EventArgs e)
    {
        RefreshPorts();
    }

    private void BtnRefreshPorts_Click(object? sender, EventArgs e)
    {
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

    #region 请求配置事件

    private void CmbFunction_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateWriteUI();
    }

    private void BtnRead_Click(object? sender, EventArgs e)
    {
        ReadData();
    }

    private void BtnSend_Click(object? sender, EventArgs e)
    {
        WriteData();
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
            LogLevel.Info => Color.LimeGreen,
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

    #region UI 状态管理

    private bool IsRtuMode => rbRtu.Checked;

    private bool IsConnected => IsRtuMode
        ? (_simulator?.IsConnected ?? _serialPort?.IsConnected ?? false)
        : (_tcpClient?.IsConnected ?? false);

    private void UpdateModeUI()
    {
        bool isRtu = rbRtu.Checked;
        lblPort.Visible = isRtu;
        cmbPort.Visible = isRtu;
        btnRefreshPorts.Visible = isRtu;
        lblBaudRate.Visible = isRtu;
        cmbBaudRate.Visible = isRtu;
        chkSimulation.Visible = isRtu;

        lblHost.Visible = !isRtu;
        txtHost.Visible = !isRtu;
        lblTcpPort.Visible = !isRtu;
        nudTcpPort.Visible = !isRtu;

        RefreshPorts();
    }

    private void UpdateWriteUI()
    {
        byte fc = GetSelectedFunctionCode();
        bool isWrite = fc == 0x05 || fc == 0x06 || fc == 0x0F || fc == 0x10;
        lblValue.Visible = isWrite;
        txtValue.Visible = isWrite;
        btnSend.Visible = isWrite;
        nudQuantity.Enabled = fc != 0x05 && fc != 0x06;
    }

    private byte GetSelectedFunctionCode()
    {
        return cmbFunction.SelectedIndex switch
        {
            0 => 0x01,
            1 => 0x02,
            2 => 0x03,
            3 => 0x04,
            4 => 0x05,
            5 => 0x06,
            6 => 0x0F,
            7 => 0x10,
            _ => 0x03
        };
    }

    private void RefreshPorts()
    {
        cmbPort.Items.Clear();
        if (rbRtu.Checked)
        {
            string[] ports = chkSimulation.Checked
                ? SerialPortSimulator.GetAvailablePorts()
                : SerialPortManager.GetAvailablePorts();
            cmbPort.Items.AddRange(ports);
            if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
        }
    }

    private void UpdateConnectionUI(bool connected)
    {
        if (InvokeRequired) { Invoke(() => UpdateConnectionUI(connected)); return; }

        if (connected)
        {
            btnConnect.Text = "断开";
            btnConnect.BackColor = Color.FromArgb(200, 50, 50);
            lblStatus.Text = "已连接";
            lblStatus.ForeColor = Color.Green;
        }
        else
        {
            btnConnect.Text = "连接";
            btnConnect.BackColor = Color.FromArgb(0, 120, 215);
            lblStatus.Text = "未连接";
            lblStatus.ForeColor = Color.Red;
        }
        SetConfigEnabled(!connected);
    }

    private void SetConfigEnabled(bool enabled)
    {
        rbRtu.Enabled = enabled;
        rbTcp.Enabled = enabled;
        cmbPort.Enabled = enabled;
        btnRefreshPorts.Enabled = enabled;
        cmbBaudRate.Enabled = enabled;
        chkSimulation.Enabled = enabled;
        txtHost.Enabled = enabled;
        nudTcpPort.Enabled = enabled;
        nudSlaveAddr.Enabled = enabled;
    }

    #endregion

    #region 连接管理

    private void Connect()
    {
        if (IsRtuMode)
            ConnectRtu();
        else
            ConnectTcp();
    }

    private void ConnectRtu()
    {
        if (cmbPort.SelectedIndex < 0)
        {
            MessageBox.Show("请选择串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var config = new SerialPortConfig
        {
            PortName = cmbPort.SelectedItem!.ToString()!.Split(' ')[0],
            BaudRate = int.Parse(cmbBaudRate.SelectedItem?.ToString() ?? "9600"),
        };
        var modbusConfig = new ModbusConfig { SlaveAddress = (byte)nudSlaveAddr.Value };

        if (chkSimulation.Checked)
        {
            _simulator = new SerialPortSimulator(_logger);
            _simulator.Mode = SimulationMode.ModbusSlave;
            _simulator.Open(config);
            _rtuClient = new ModbusRtuClient(_simulator, _logger, modbusConfig);
        }
        else
        {
            _serialPort = new SerialPortManager(_logger);
            if (!_serialPort.Open(config)) return;
            _rtuClient = new ModbusRtuClient(_serialPort, _logger, modbusConfig);
        }

        UpdateConnectionUI(true);
    }

    private void ConnectTcp()
    {
        var tcpConfig = new TcpConfig
        {
            Host = txtHost.Text.Trim(),
            Port = (int)nudTcpPort.Value
        };
        var modbusConfig = new ModbusConfig { SlaveAddress = (byte)nudSlaveAddr.Value };

        _tcpManager = new TcpClientManager(_logger, tcpConfig);
        _tcpClient = new ModbusTcpClient(_tcpManager, _logger, modbusConfig);

        _ = Task.Run(async () =>
        {
            bool success = await _tcpClient.ConnectAsync();
            Invoke(() => UpdateConnectionUI(success));
        });
    }

    private void Disconnect()
    {
        _rtuClient?.Dispose();
        _rtuClient = null;
        _simulator?.Close();
        _simulator = null;
        _serialPort?.Close();
        _serialPort = null;
        _tcpClient?.Dispose();
        _tcpClient = null;
        _tcpManager?.Dispose();
        _tcpManager = null;
        UpdateConnectionUI(false);
    }

    #endregion

    #region 数据读写

    private async void ReadData()
    {
        try
        {
            if (!IsConnected) { MessageBox.Show("请先连接设备", "提示"); return; }

            ushort startAddr = (ushort)nudStartAddr.Value;
            ushort quantity = (ushort)nudQuantity.Value;
            byte fc = GetSelectedFunctionCode();
            ModbusResponse? response = null;

            try
            {
                if (IsRtuMode)
                {
                    response = fc switch
                    {
                        0x01 => _rtuClient!.ReadCoils(startAddr, quantity),
                        0x02 => _rtuClient!.ReadDiscreteInputs(startAddr, quantity),
                        0x03 => _rtuClient!.ReadHoldingRegisters(startAddr, quantity),
                        0x04 => _rtuClient!.ReadInputRegisters(startAddr, quantity),
                        _ => null
                    };
                }
                else
                {
                    response = fc switch
                    {
                        0x01 => await _tcpClient!.ReadCoilsAsync(startAddr, quantity),
                        0x03 => await _tcpClient!.ReadHoldingRegistersAsync(startAddr, quantity),
                        0x04 => await _tcpClient!.ReadInputRegistersAsync(startAddr, quantity),
                        _ => null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"读取异常: {ex.Message}", nameof(ModbusControl));
            }

            if (response != null)
                DisplayResponse(response, startAddr);
            else
                _logger.Warning("未收到响应", nameof(ModbusControl));
        }
        catch (Exception ex)
        {
            _logger.Error($"ReadData 严重错误: {ex}", nameof(ModbusControl));
        }
    }

    private async void WriteData()
    {
        try
        {
            if (!IsConnected) { MessageBox.Show("请先连接设备", "提示"); return; }

            ushort startAddr = (ushort)nudStartAddr.Value;
            byte fc = GetSelectedFunctionCode();
            string valueText = txtValue.Text.Trim();
            ModbusResponse? response = null;

            try
            {
                if (IsRtuMode)
                {
                    response = fc switch
                    {
                        0x05 => _rtuClient!.WriteSingleCoil(startAddr, valueText == "1" || valueText.ToLower() == "true"),
                        0x06 => _rtuClient!.WriteSingleRegister(startAddr, ushort.Parse(valueText)),
                        0x10 => _rtuClient!.WriteMultipleRegisters(startAddr, ParseUshortArray(valueText)),
                        _ => null
                    };
                }
                else
                {
                    response = fc switch
                    {
                        0x05 => await _tcpClient!.WriteSingleCoilAsync(startAddr, valueText == "1" || valueText.ToLower() == "true"),
                        0x06 => await _tcpClient!.WriteSingleRegisterAsync(startAddr, ushort.Parse(valueText)),
                        0x10 => await _tcpClient!.WriteMultipleRegistersAsync(startAddr, ParseUshortArray(valueText)),
                        _ => null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"写入异常: {ex.Message}", nameof(ModbusControl));
            }

            if (response?.Success == true)
                _logger.Info("写入成功", nameof(ModbusControl));
            else if (response != null)
                _logger.Warning($"写入失败: {response.ErrorMessage}", nameof(ModbusControl));
        }
        catch (Exception ex)
        {
            _logger.Error($"WriteData 严重错误: {ex}", nameof(ModbusControl));
        }
    }

    private ushort[] ParseUshortArray(string text)
    {
        return text.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => ushort.Parse(s.Trim()))
            .ToArray();
    }

    private void DisplayResponse(ModbusResponse response, ushort startAddr)
    {
        dgvRegisters.Rows.Clear();

        if (!response.Success)
        {
            _logger.Warning($"读取失败: {response.ErrorMessage}", nameof(ModbusControl));
            return;
        }

        if (response.RegisterValues != null)
        {
            string dataType = cmbDataType.SelectedItem?.ToString() ?? "UInt16";
            for (int i = 0; i < response.RegisterValues.Length; i++)
            {
                ushort raw = response.RegisterValues[i];
                string parsed = dataType switch
                {
                    "UInt16" => raw.ToString(),
                    "Int16" => ((short)raw).ToString(),
                    "Hex" => $"0x{raw:X4}",
                    "ASCII" => $"{(char)(raw >> 8)}{(char)(raw & 0xFF)}",
                    "UInt32" when i + 1 < response.RegisterValues.Length => response.GetInt32(i).ToString(),
                    "Float32" when i + 1 < response.RegisterValues.Length => response.GetFloat32(i).ToString("F4"),
                    _ => raw.ToString()
                };
                dgvRegisters.Rows.Add($"{startAddr + i}", $"0x{raw:X4}", parsed);
            }
        }
        else if (response.CoilValues != null)
        {
            for (int i = 0; i < response.CoilValues.Length; i++)
            {
                dgvRegisters.Rows.Add($"{startAddr + i}", response.CoilValues[i] ? "1" : "0", response.CoilValues[i] ? "ON" : "OFF");
            }
        }
    }

    #endregion
}
