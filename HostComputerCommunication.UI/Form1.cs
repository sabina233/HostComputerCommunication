using HostComputerCommunication.UI.Controls;

namespace HostComputerCommunication.UI;

public partial class Form1 : Form
{
    private TabControl tabMain = null!;

    public Form1()
    {
        InitializeComponent();
        SetupUI();
    }

    private void SetupUI()
    {
        Text = "上位机通信工具集";
        Size = new Size(920, 750);
        MinimumSize = new Size(920, 750);
        StartPosition = FormStartPosition.CenterScreen;

        tabMain = new TabControl
        {
            Dock = DockStyle.Fill
        };

        // 串口调试助手
        var tabSerial = new TabPage("串口调试助手");
        tabSerial.Controls.Add(new SerialPortControl());
        tabMain.TabPages.Add(tabSerial);

        // Modbus 调试工具
        var tabModbus = new TabPage("Modbus 调试工具");
        tabModbus.Controls.Add(new ModbusControl());
        tabMain.TabPages.Add(tabModbus);

        // TCP 通信工具
        var tabTcp = new TabPage("TCP 通信工具");
        tabTcp.Controls.Add(new TcpSocketControl());
        tabMain.TabPages.Add(tabTcp);

        // OPC UA 客户端
        var tabOpcUa = new TabPage("OPC UA 客户端");
        tabOpcUa.Controls.Add(new OpcUaControl());
        tabMain.TabPages.Add(tabOpcUa);

        Controls.Add(tabMain);
    }
}
