using System.Text;

namespace HostComputerCommunication.Common.Helpers;

/// <summary>
/// 字节操作工具类
/// 提供字节数组与十六进制字符串互转、大小端序转换等功能
/// </summary>
public static class ByteHelper
{
    /// <summary>
    /// 字节数组转十六进制字符串
    /// </summary>
    /// <param name="data">字节数组</param>
    /// <param name="separator">分隔符，默认空格</param>
    /// <returns>格式化后的十六进制字符串，如 "01 03 FF"</returns>
    public static string ToHexString(byte[] data, string separator = " ")
    {
        if (data == null || data.Length == 0) return string.Empty;
        return string.Join(separator, data.Select(b => b.ToString("X2")));
    }

    /// <summary>
    /// 十六进制字符串转字节数组
    /// 支持带空格和不带空格的格式，如 "01 03 FF" 或 "0103FF"
    /// </summary>
    /// <param name="hex">十六进制字符串</param>
    /// <returns>字节数组</returns>
    /// <exception cref="ArgumentException">字符串长度为奇数时抛出</exception>
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
    /// 大端序字节数组转 short（有符号16位整数）
    /// </summary>
    /// <param name="data">字节数组</param>
    /// <param name="offset">起始偏移量</param>
    /// <returns>short 值</returns>
    public static short ToInt16BigEndian(byte[] data, int offset = 0)
    {
        return (short)((data[offset] << 8) | data[offset + 1]);
    }

    /// <summary>
    /// 大端序字节数组转 ushort（无符号16位整数）
    /// </summary>
    public static ushort ToUInt16BigEndian(byte[] data, int offset = 0)
    {
        return (ushort)((data[offset] << 8) | data[offset + 1]);
    }

    /// <summary>
    /// 大端序字节数组转 int（有符号32位整数）
    /// </summary>
    public static int ToInt32BigEndian(byte[] data, int offset = 0)
    {
        return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
    }

    /// <summary>
    /// 大端序字节数组转 float（32位浮点数，IEEE 754）
    /// Modbus 寄存器常用此方法解析浮点数据
    /// </summary>
    public static float ToFloatBigEndian(byte[] data, int offset = 0)
    {
        // 大端序需要反转字节序再转 float
        byte[] reversed = new byte[4];
        reversed[0] = data[offset + 3];
        reversed[1] = data[offset + 2];
        reversed[2] = data[offset + 1];
        reversed[3] = data[offset];
        return BitConverter.ToSingle(reversed, 0);
    }

    /// <summary>
    /// 小端序字节数组转 float
    /// </summary>
    public static float ToFloatLittleEndian(byte[] data, int offset = 0)
    {
        return BitConverter.ToSingle(data, offset);
    }

    /// <summary>
    /// short 转大端序字节数组（2字节）
    /// </summary>
    public static byte[] GetBytesBigEndian(short value)
    {
        return [(byte)(value >> 8), (byte)value];
    }

    /// <summary>
    /// ushort 转大端序字节数组（2字节）
    /// </summary>
    public static byte[] GetBytesBigEndian(ushort value)
    {
        return [(byte)(value >> 8), (byte)value];
    }

    /// <summary>
    /// int 转大端序字节数组（4字节）
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
