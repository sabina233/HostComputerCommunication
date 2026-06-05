using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Modbus.Protocol;

namespace HostComputerCommunication.Tests;

public class ModbusResponseParserTests
{
    [Fact]
    public void ParseRtuResponse_ReadHoldingRegisters()
    {
        // 从站1, 功能码03, 2字节数据, 值=100
        byte[] dataWithoutCrc = [0x01, 0x03, 0x02, 0x00, 0x64];
        byte[] crc = CrcHelper.Crc16ModbusBytes(dataWithoutCrc, 0, dataWithoutCrc.Length);
        byte[] data = [.. dataWithoutCrc, .. crc];

        var response = ModbusResponseParser.ParseRtuResponse(data);

        Assert.True(response.Success);
        Assert.Equal(1, response.SlaveAddress);
        Assert.Equal(0x03, response.FunctionCode);
        Assert.NotNull(response.RegisterValues);
        Assert.Single(response.RegisterValues);
        Assert.Equal(100, response.RegisterValues[0]);
    }

    [Fact]
    public void ParseRtuResponse_MultipleRegisters()
    {
        // 从站1, 功能码03, 4字节数据, 值=[100, 200]
        byte[] dataWithoutCrc = [0x01, 0x03, 0x04, 0x00, 0x64, 0x00, 0xC8];
        byte[] crc = CrcHelper.Crc16ModbusBytes(dataWithoutCrc, 0, dataWithoutCrc.Length);
        byte[] data = [.. dataWithoutCrc, .. crc];

        var response = ModbusResponseParser.ParseRtuResponse(data);

        Assert.True(response.Success);
        Assert.NotNull(response.RegisterValues);
        Assert.Equal(2, response.RegisterValues.Length);
        Assert.Equal(100, response.RegisterValues[0]);
        Assert.Equal(200, response.RegisterValues[1]);
    }

    [Fact]
    public void ParseRtuResponse_CrcError()
    {
        // 故意使用错误的 CRC
        byte[] data = [0x01, 0x03, 0x02, 0x00, 0x64, 0x00, 0x00];
        var response = ModbusResponseParser.ParseRtuResponse(data);

        Assert.False(response.Success);
        Assert.Contains("CRC", response.ErrorMessage);
    }

    [Fact]
    public void ParseRtuResponse_ExceptionResponse()
    {
        // 异常响应: 从站1, 功能码0x83, 异常码02
        byte[] dataWithoutCrc = [0x01, 0x83, 0x02];
        byte[] crc = CrcHelper.Crc16ModbusBytes(dataWithoutCrc, 0, dataWithoutCrc.Length);
        byte[] data = [.. dataWithoutCrc, .. crc];

        var response = ModbusResponseParser.ParseRtuResponse(data);

        Assert.False(response.Success);
        Assert.Contains("非法数据地址", response.ErrorMessage);
    }

    [Fact]
    public void ParseRtuResponse_WriteSingleRegister()
    {
        byte[] dataWithoutCrc = [0x01, 0x06, 0x00, 0x64, 0x04, 0xD2];
        byte[] crc = CrcHelper.Crc16ModbusBytes(dataWithoutCrc, 0, dataWithoutCrc.Length);
        byte[] data = [.. dataWithoutCrc, .. crc];

        var response = ModbusResponseParser.ParseRtuResponse(data);

        Assert.True(response.Success);
        Assert.Equal(0x06, response.FunctionCode);
    }

    [Fact]
    public void ModbusResponse_GetInt16()
    {
        var response = new ModbusResponse
        {
            Success = true,
            RegisterValues = [0x0064] // 100
        };
        Assert.Equal(100, response.GetInt16(0));
    }

    [Fact]
    public void ModbusResponse_GetUInt16()
    {
        var response = new ModbusResponse
        {
            Success = true,
            RegisterValues = [0xFFFF] // 65535
        };
        Assert.Equal(65535, response.GetUInt16(0));
    }

    [Fact]
    public void ModbusResponse_GetInt32()
    {
        var response = new ModbusResponse
        {
            Success = true,
            RegisterValues = [0x0001, 0x86A0] // 100000
        };
        Assert.Equal(100000, response.GetInt32(0));
    }

    [Fact]
    public void ModbusResponse_GetString()
    {
        var response = new ModbusResponse
        {
            Success = true,
            RegisterValues = [0x4869] // "Hi"
        };
        Assert.Equal("Hi", response.GetString(0, 1));
    }
}
