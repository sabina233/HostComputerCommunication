using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Modbus.Protocol;

namespace HostComputerCommunication.Tests;

public class ModbusFrameBuilderTests
{
    [Fact]
    public void RtuReadHoldingRegisters_CorrectFrame()
    {
        byte[] frame = ModbusFrameBuilder.RtuReadHoldingRegisters(1, 0, 10);
        Assert.Equal(8, frame.Length);
        Assert.Equal(0x01, frame[0]); // 从站地址
        Assert.Equal(0x03, frame[1]); // 功能码
        Assert.Equal(0x00, frame[2]); // 起始地址高
        Assert.Equal(0x00, frame[3]); // 起始地址低
        Assert.Equal(0x00, frame[4]); // 数量高
        Assert.Equal(0x0A, frame[5]); // 数量低
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame));
    }

    [Fact]
    public void RtuWriteSingleRegister_CorrectFrame()
    {
        byte[] frame = ModbusFrameBuilder.RtuWriteSingleRegister(1, 100, 1234);
        Assert.Equal(8, frame.Length);
        Assert.Equal(0x01, frame[0]);
        Assert.Equal(0x06, frame[1]);
        Assert.Equal(0x00, frame[2]); // 地址 100 高
        Assert.Equal(0x64, frame[3]); // 地址 100 低
        Assert.Equal(0x04, frame[4]); // 值 1234 高
        Assert.Equal(0xD2, frame[5]); // 值 1234 低
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame));
    }

    [Fact]
    public void RtuReadCoils_CorrectFrame()
    {
        byte[] frame = ModbusFrameBuilder.RtuReadCoils(1, 0, 8);
        Assert.Equal(8, frame.Length);
        Assert.Equal(0x01, frame[0]);
        Assert.Equal(0x01, frame[1]);
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame));
    }

    [Fact]
    public void TcpReadHoldingRegisters_CorrectFrame()
    {
        byte[] frame = ModbusFrameBuilder.TcpReadHoldingRegisters(1, 1, 0, 10);
        Assert.Equal(12, frame.Length);
        Assert.Equal(0x00, frame[0]); // Transaction ID high
        Assert.Equal(0x01, frame[1]); // Transaction ID low
        Assert.Equal(0x00, frame[2]); // Protocol ID
        Assert.Equal(0x00, frame[3]);
        Assert.Equal(0x00, frame[4]); // Length high
        Assert.Equal(0x06, frame[5]); // Length low
        Assert.Equal(0x01, frame[6]); // Unit ID
        Assert.Equal(0x03, frame[7]); // Function code
    }

    [Fact]
    public void RtuWriteMultipleRegisters_CorrectFrame()
    {
        ushort[] values = [100, 200, 300];
        byte[] frame = ModbusFrameBuilder.RtuWriteMultipleRegisters(1, 0, values);
        Assert.Equal(15, frame.Length); // 7 header + 6 data + 2 CRC
        Assert.Equal(0x01, frame[0]);
        Assert.Equal(0x10, frame[1]);
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame));
    }

    [Fact]
    public void GetFunctionName_ReturnsCorrectNames()
    {
        Assert.Equal("读保持寄存器 (03)", ModbusFrameBuilder.GetFunctionName(0x03));
        Assert.Equal("写单个寄存器 (06)", ModbusFrameBuilder.GetFunctionName(0x06));
    }

    [Fact]
    public void RtuFrame_RoundTrip_CrcValid()
    {
        // 测试所有读功能码的帧都能通过 CRC 验证
        byte[] frame1 = ModbusFrameBuilder.RtuReadCoils(1, 0, 1);
        byte[] frame2 = ModbusFrameBuilder.RtuReadDiscreteInputs(1, 0, 1);
        byte[] frame3 = ModbusFrameBuilder.RtuReadHoldingRegisters(1, 0, 1);
        byte[] frame4 = ModbusFrameBuilder.RtuReadInputRegisters(1, 0, 1);

        Assert.True(CrcHelper.VerifyCrc16Modbus(frame1));
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame2));
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame3));
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame4));
    }
}
