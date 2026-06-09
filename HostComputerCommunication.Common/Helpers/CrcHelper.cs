namespace HostComputerCommunication.Common.Helpers;

/// <summary>
/// CRC 校验工具类
/// 提供 CRC16 Modbus 校验算法，用于 Modbus RTU 通信的数据完整性验证
/// </summary>
public static class CrcHelper
{
    /// <summary>
    /// 计算 CRC16 Modbus 校验值
    /// 多项式: 0xA001（反转的 0x8005）
    /// </summary>
    /// <param name="data">待校验的字节数组</param>
    /// <param name="offset">起始偏移量</param>
    /// <param name="length">校验长度</param>
    /// <returns>CRC16 校验值</returns>
    public static ushort Crc16Modbus(byte[] data, int offset, int length)
    {
        ushort crc = 0xFFFF; // 初始值
        for (int i = offset; i < offset + length; i++)
        {
            crc ^= data[i];
            for (int j = 0; j < 8; j++)
            {
                if ((crc & 0x0001) != 0)
                    crc = (ushort)((crc >> 1) ^ 0xA001);
                else
                    crc >>= 1;
            }
        }
        return crc;
    }

    /// <summary>
    /// 计算整个字节数组的 CRC16 Modbus 校验值
    /// </summary>
    public static ushort Crc16Modbus(byte[] data)
    {
        return Crc16Modbus(data, 0, data.Length);
    }

    /// <summary>
    /// 计算 CRC16 Modbus 并返回字节数组（低字节在前，高字节在后）
    /// Modbus RTU 协议要求 CRC 低字节在前
    /// </summary>
    public static byte[] Crc16ModbusBytes(byte[] data, int offset, int length)
    {
        ushort crc = Crc16Modbus(data, offset, length);
        return [(byte)(crc & 0xFF), (byte)(crc >> 8)];
    }

    /// <summary>
    /// 验证 Modbus RTU 帧的 CRC 校验是否正确
    /// 帧末尾2字节为 CRC 校验码（低字节在前）
    /// </summary>
    /// <param name="data">完整的 Modbus RTU 帧</param>
    /// <returns>校验通过返回 true</returns>
    public static bool VerifyCrc16Modbus(byte[] data)
    {
        if (data.Length < 3) return false;
        ushort calculated = Crc16Modbus(data, 0, data.Length - 2);
        ushort received = (ushort)(data[data.Length - 2] | (data[data.Length - 1] << 8));
        return calculated == received;
    }
}
