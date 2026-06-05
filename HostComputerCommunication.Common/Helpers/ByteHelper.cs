using System.Text;

namespace HostComputerCommunication.Common.Helpers;

/// <summary>
/// 字节操作工具类
/// </summary>
public static class ByteHelper
{
    /// <summary>
    /// 字节数组转十六进制字符串
    /// </summary>
    public static string ToHexString(byte[] data, string separator = " ")
    {
        if (data == null || data.Length == 0) return string.Empty;
        return string.Join(separator, data.Select(b => b.ToString("X2")));
    }

    /// <summary>
    /// 十六进制字符串转字节数组
    /// </summary>
    public static byte[] FromHexString(string hex)
    {
        hex = hex.Replace(" ", "").Replace("-", "");
        if (hex.Length % 2 != 0)
            throw new ArgumentException("十六进制字符串长度必须为偶数");

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    /// <summary>
    /// 大端序转short
    /// </summary>
    public static short ToInt16BigEndian(byte[] data, int offset = 0)
    {
        return (short)((data[offset] << 8) | data[offset + 1]);
    }

    /// <summary>
    /// 大端序转ushort
    /// </summary>
    public static ushort ToUInt16BigEndian(byte[] data, int offset = 0)
    {
        return (ushort)((data[offset] << 8) | data[offset + 1]);
    }

    /// <summary>
    /// 大端序转int
    /// </summary>
    public static int ToInt32BigEndian(byte[] data, int offset = 0)
    {
        return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
    }

    /// <summary>
    /// 大端序转float (IEEE 754)
    /// </summary>
    public static float ToFloatBigEndian(byte[] data, int offset = 0)
    {
        byte[] reversed = new byte[4];
        reversed[0] = data[offset + 3];
        reversed[1] = data[offset + 2];
        reversed[2] = data[offset + 1];
        reversed[3] = data[offset];
        return BitConverter.ToSingle(reversed, 0);
    }

    /// <summary>
    /// 小端序转float
    /// </summary>
    public static float ToFloatLittleEndian(byte[] data, int offset = 0)
    {
        return BitConverter.ToSingle(data, offset);
    }

    /// <summary>
    /// short转大端序字节数组
    /// </summary>
    public static byte[] GetBytesBigEndian(short value)
    {
        return [(byte)(value >> 8), (byte)value];
    }

    /// <summary>
    /// ushort转大端序字节数组
    /// </summary>
    public static byte[] GetBytesBigEndian(ushort value)
    {
        return [(byte)(value >> 8), (byte)value];
    }

    /// <summary>
    /// int转大端序字节数组
    /// </summary>
    public static byte[] GetBytesBigEndian(int value)
    {
        return
        [
            (byte)(value >> 24),
            (byte)(value >> 16),
            (byte)(value >> 8),
            (byte)value
        ];
    }
}
