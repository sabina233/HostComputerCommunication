using HostComputerCommunication.Common.Helpers;

namespace HostComputerCommunication.Tests;

public class ByteHelperTests
{
    [Fact]
    public void ToHexString_ReturnsCorrectFormat()
    {
        byte[] data = [0x01, 0x03, 0xFF, 0xAB];
        string result = ByteHelper.ToHexString(data);
        Assert.Equal("01 03 FF AB", result);
    }

    [Fact]
    public void ToHexString_CustomSeparator()
    {
        byte[] data = [0x01, 0x02];
        string result = ByteHelper.ToHexString(data, "-");
        Assert.Equal("01-02", result);
    }

    [Fact]
    public void ToHexString_EmptyArray()
    {
        Assert.Equal(string.Empty, ByteHelper.ToHexString([]));
    }

    [Fact]
    public void FromHexString_ParsesCorrectly()
    {
        byte[] result = ByteHelper.FromHexString("01 03 FF AB");
        Assert.Equal([0x01, 0x03, 0xFF, 0xAB], result);
    }

    [Fact]
    public void FromHexString_NoSpaces()
    {
        byte[] result = ByteHelper.FromHexString("0103FFAB");
        Assert.Equal([0x01, 0x03, 0xFF, 0xAB], result);
    }

    [Fact]
    public void FromHexString_InvalidLength_Throws()
    {
        Assert.Throws<ArgumentException>(() => ByteHelper.FromHexString("010"));
    }

    [Fact]
    public void ToInt16BigEndian_WorksCorrectly()
    {
        byte[] data = [0x00, 0x64]; // 100
        Assert.Equal(100, ByteHelper.ToInt16BigEndian(data));
    }

    [Fact]
    public void ToUInt16BigEndian_WorksCorrectly()
    {
        byte[] data = [0xFF, 0xFF]; // 65535
        Assert.Equal(65535, ByteHelper.ToUInt16BigEndian(data));
    }

    [Fact]
    public void GetBytesBigEndian_Int16()
    {
        byte[] result = ByteHelper.GetBytesBigEndian((short)256);
        Assert.Equal([0x01, 0x00], result);
    }

    [Fact]
    public void ToFloatBigEndian_WorksCorrectly()
    {
        // 1.0f in big endian: 3F 80 00 00
        byte[] data = [0x3F, 0x80, 0x00, 0x00];
        float result = ByteHelper.ToFloatBigEndian(data);
        Assert.Equal(1.0f, result);
    }
}
