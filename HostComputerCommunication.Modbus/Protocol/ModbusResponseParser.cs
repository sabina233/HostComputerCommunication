using HostComputerCommunication.Common.Helpers;

namespace HostComputerCommunication.Modbus.Protocol;

/// <summary>
/// Modbus 响应解析结果
/// 包含解析后的寄存器值、线圈状态，以及多种数据类型转换方法
/// </summary>
public class ModbusResponse
{
    public bool Success { get; set; }
    public byte SlaveAddress { get; set; }
    public byte FunctionCode { get; set; }
    public string? ErrorMessage { get; set; }

    // 读寄存器结果
    public ushort[]? RegisterValues { get; set; }

    // 读线圈结果
    public bool[]? CoilValues { get; set; }

    // 原始数据
    public byte[] RawData { get; set; } = [];

    // 数据类型解析
    public short GetInt16(int index) => RegisterValues != null ? (short)RegisterValues[index] : (short)0;
    public ushort GetUInt16(int index) => RegisterValues?[index] ?? 0;

    public int GetInt32(int startIndex)
    {
        if (RegisterValues == null || startIndex + 1 >= RegisterValues.Length) return 0;
        return (RegisterValues[startIndex] << 16) | RegisterValues[startIndex + 1];
    }

    public float GetFloat32(int startIndex)
    {
        if (RegisterValues == null || startIndex + 1 >= RegisterValues.Length) return 0;
        byte[] bytes =
        [
            (byte)(RegisterValues[startIndex] >> 8),
            (byte)RegisterValues[startIndex],
            (byte)(RegisterValues[startIndex + 1] >> 8),
            (byte)RegisterValues[startIndex + 1]
        ];
        return BitConverter.ToSingle(bytes, 0);
    }

    public string GetString(int startIndex, int registerCount)
    {
        if (RegisterValues == null) return string.Empty;
        byte[] bytes = new byte[registerCount * 2];
        for (int i = 0; i < registerCount && startIndex + i < RegisterValues.Length; i++)
        {
            bytes[i * 2] = (byte)(RegisterValues[startIndex + i] >> 8);
            bytes[i * 2 + 1] = (byte)RegisterValues[startIndex + i];
        }
        return System.Text.Encoding.ASCII.GetString(bytes).TrimEnd('\0');
    }
}

/// <summary>
/// Modbus 响应解析器
/// 解析 Modbus RTU/TCP 响应帧，提取寄存器数据和线圈状态
/// 支持异常响应解析
/// </summary>
public static class ModbusResponseParser
{
    /// <summary>
    /// 解析 RTU 响应
    /// </summary>
    public static ModbusResponse ParseRtuResponse(byte[] data)
    {
        var response = new ModbusResponse { RawData = data };

        if (data.Length < 4)
        {
            response.Success = false;
            response.ErrorMessage = "响应数据太短";
            return response;
        }

        // 验证 CRC
        if (!CrcHelper.VerifyCrc16Modbus(data))
        {
            response.Success = false;
            response.ErrorMessage = "CRC 校验失败";
            return response;
        }

        response.SlaveAddress = data[0];
        response.FunctionCode = data[1];

        // 检查异常响应
        if ((data[1] & 0x80) != 0)
        {
            response.Success = false;
            response.ErrorMessage = GetExceptionMessage(data[2]);
            return response;
        }

        return ParsePdu(response, data, 1);
    }

    /// <summary>
    /// 解析 TCP 响应
    /// </summary>
    public static ModbusResponse ParseTcpResponse(byte[] data)
    {
        var response = new ModbusResponse { RawData = data };

        if (data.Length < 9) // MBAP(7) + at least 2 bytes PDU
        {
            response.Success = false;
            response.ErrorMessage = "响应数据太短";
            return response;
        }

        // MBAP Header
        ushort transactionId = (ushort)((data[0] << 8) | data[1]);
        ushort length = (ushort)((data[4] << 8) | data[5]);
        response.SlaveAddress = data[6];
        response.FunctionCode = data[7];

        // 检查异常响应
        if ((data[7] & 0x80) != 0)
        {
            response.Success = false;
            response.ErrorMessage = data.Length > 8 ? GetExceptionMessage(data[8]) : "未知异常";
            return response;
        }

        return ParsePdu(response, data, 7);
    }

    private static ModbusResponse ParsePdu(ModbusResponse response, byte[] data, int pduOffset)
    {
        byte fc = response.FunctionCode;

        switch (fc)
        {
            case 0x01: // Read Coils
            case 0x02: // Read Discrete Inputs
                if (data.Length < pduOffset + 2)
                {
                    response.Success = false;
                    response.ErrorMessage = "数据不完整";
                    return response;
                }
                int coilByteCount = data[pduOffset + 1];
                response.CoilValues = new bool[coilByteCount * 8];
                for (int i = 0; i < coilByteCount * 8; i++)
                {
                    int byteIdx = pduOffset + 2 + i / 8;
                    if (byteIdx < data.Length)
                        response.CoilValues[i] = (data[byteIdx] & (1 << (i % 8))) != 0;
                }
                response.Success = true;
                break;

            case 0x03: // Read Holding Registers
            case 0x04: // Read Input Registers
                if (data.Length < pduOffset + 2)
                {
                    response.Success = false;
                    response.ErrorMessage = "数据不完整";
                    return response;
                }
                int regByteCount = data[pduOffset + 1];
                int regCount = regByteCount / 2;
                response.RegisterValues = new ushort[regCount];
                for (int i = 0; i < regCount; i++)
                {
                    int hi = pduOffset + 2 + i * 2;
                    int lo = hi + 1;
                    if (lo < data.Length)
                        response.RegisterValues[i] = (ushort)((data[hi] << 8) | data[lo]);
                }
                response.Success = true;
                break;

            case 0x05: // Write Single Coil
            case 0x06: // Write Single Register
            case 0x0F: // Write Multiple Coils
            case 0x10: // Write Multiple Registers
                response.Success = true;
                break;

            default:
                response.Success = false;
                response.ErrorMessage = $"不支持的功能码: 0x{fc:X2}";
                break;
        }

        return response;
    }

    private static string GetExceptionMessage(byte exceptionCode)
    {
        return exceptionCode switch
        {
            0x01 => "非法功能码",
            0x02 => "非法数据地址",
            0x03 => "非法数据值",
            0x04 => "从站设备故障",
            0x05 => "确认（长周期）",
            0x06 => "从站设备忙",
            0x08 => "存储奇偶性差错",
            0x0A => "不可用网关路径",
            0x0B => "网关目标设备响应失败",
            _ => $"未知异常码 (0x{exceptionCode:X2})"
        };
    }
}
