using HostComputerCommunication.Common.Helpers;

namespace HostComputerCommunication.Tests;

public class CrcHelperTests
{
    [Fact]
    public void Crc16Modbus_CalculatesConsistently()
    {
        byte[] data = [0x01, 0x03, 0x00, 0x00, 0x00, 0x01];
        ushort crc1 = CrcHelper.Crc16Modbus(data);
        ushort crc2 = CrcHelper.Crc16Modbus(data);
        Assert.Equal(crc1, crc2);
        Assert.NotEqual(0, crc1);
    }

    [Fact]
    public void Crc16ModbusBytes_MatchesDirectCalculation()
    {
        byte[] data = [0x01, 0x03, 0x00, 0x00, 0x00, 0x01];
        byte[] crcBytes = CrcHelper.Crc16ModbusBytes(data, 0, data.Length);
        ushort crcFromBytes = (ushort)(crcBytes[0] | (crcBytes[1] << 8));
        ushort directCrc = CrcHelper.Crc16Modbus(data);
        Assert.Equal(directCrc, crcFromBytes);
    }

    [Fact]
    public void VerifyCrc16Modbus_ValidFrame()
    {
        byte[] dataWithoutCrc = [0x01, 0x03, 0x00, 0x00, 0x00, 0x01];
        byte[] crc = CrcHelper.Crc16ModbusBytes(dataWithoutCrc, 0, dataWithoutCrc.Length);
        byte[] frame = [.. dataWithoutCrc, .. crc];
        Assert.True(CrcHelper.VerifyCrc16Modbus(frame));
    }

    [Fact]
    public void VerifyCrc16Modbus_InvalidFrame()
    {
        byte[] frame = [0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00];
        Assert.False(CrcHelper.VerifyCrc16Modbus(frame));
    }

    [Fact]
    public void VerifyCrc16Modbus_TooShort()
    {
        Assert.False(CrcHelper.VerifyCrc16Modbus([0x01]));
    }

    [Fact]
    public void Crc16Modbus_DifferentData_DifferentCrc()
    {
        byte[] data1 = [0x01, 0x03, 0x00, 0x00, 0x00, 0x01];
        byte[] data2 = [0x01, 0x03, 0x00, 0x00, 0x00, 0x02];
        ushort crc1 = CrcHelper.Crc16Modbus(data1);
        ushort crc2 = CrcHelper.Crc16Modbus(data2);
        Assert.NotEqual(crc1, crc2);
    }
}
