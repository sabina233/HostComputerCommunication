namespace HostComputerCommunication.Common.Helpers;

/// <summary>
/// CRC 校验工具类
/// </summary>
public static class CrcHelper
{
    /// <summary>
    /// CRC16 Modbus 校验
    /// </summary>
    public static ushort Crc16Modbus(byte[] data, int offset, int length)
    {
        ushort crc = 0xFFFF;
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
    /// CRC16 Modbus 校验（完整数组）
    /// </summary>
    public static ushort Crc16Modbus(byte[] data)
    {
        return Crc16Modbus(data, 0, data.Length);
    }

    /// <summary>
    /// CRC16 Modbus 校验字节（低字节在前，高字节在后）
    /// </summary>
    public static byte[] Crc16ModbusBytes(byte[] data, int offset, int length)
    {
        ushort crc = Crc16Modbus(data, offset, length);
        return [(byte)(crc & 0xFF), (byte)(crc >> 8)];
    }

    /// <summary>
    /// 验证 CRC16 Modbus 校验
    /// </summary>
    public static bool VerifyCrc16Modbus(byte[] data)
    {
        if (data.Length < 3) return false;
        ushort calculated = Crc16Modbus(data, 0, data.Length - 2);
        ushort received = (ushort)(data[data.Length - 2] | (data[data.Length - 1] << 8));
        return calculated == received;
    }
}
