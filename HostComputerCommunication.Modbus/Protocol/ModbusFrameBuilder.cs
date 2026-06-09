using HostComputerCommunication.Common.Helpers;

namespace HostComputerCommunication.Modbus.Protocol;

/// <summary>
/// Modbus 帧构建器
/// 提供 Modbus RTU 和 TCP 协议的请求帧构建方法
/// 支持所有常用功能码：01/02/03/04/05/06/0F/10
/// </summary>
public static class ModbusFrameBuilder
{
    // === RTU 帧 ===

    public static byte[] RtuReadCoils(byte slaveAddr, ushort startAddress, ushort quantity)
        => BuildRtuRequest(slaveAddr, 0x01, startAddress, quantity);

    public static byte[] RtuReadDiscreteInputs(byte slaveAddr, ushort startAddress, ushort quantity)
        => BuildRtuRequest(slaveAddr, 0x02, startAddress, quantity);

    public static byte[] RtuReadHoldingRegisters(byte slaveAddr, ushort startAddress, ushort quantity)
        => BuildRtuRequest(slaveAddr, 0x03, startAddress, quantity);

    public static byte[] RtuReadInputRegisters(byte slaveAddr, ushort startAddress, ushort quantity)
        => BuildRtuRequest(slaveAddr, 0x04, startAddress, quantity);

    public static byte[] RtuWriteSingleCoil(byte slaveAddr, ushort address, bool value)
    {
        ushort coilValue = value ? (ushort)0xFF00 : (ushort)0x0000;
        return BuildRtuRequest(slaveAddr, 0x05, address, coilValue);
    }

    public static byte[] RtuWriteSingleRegister(byte slaveAddr, ushort address, ushort value)
        => BuildRtuRequest(slaveAddr, 0x06, address, value);

    public static byte[] RtuWriteMultipleCoils(byte slaveAddr, ushort startAddress, bool[] values)
    {
        int byteCount = (values.Length + 7) / 8;
        byte[] frame = new byte[7 + byteCount + 2]; // header + data + CRC
        frame[0] = slaveAddr;
        frame[1] = 0x0F;
        frame[2] = (byte)(startAddress >> 8);
        frame[3] = (byte)startAddress;
        frame[4] = (byte)(values.Length >> 8);
        frame[5] = (byte)values.Length;
        frame[6] = (byte)byteCount;

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i])
                frame[7 + i / 8] |= (byte)(1 << (i % 8));
        }

        byte[] crc = CrcHelper.Crc16ModbusBytes(frame, 0, frame.Length - 2);
        frame[^2] = crc[0];
        frame[^1] = crc[1];
        return frame;
    }

    public static byte[] RtuWriteMultipleRegisters(byte slaveAddr, ushort startAddress, ushort[] values)
    {
        int byteCount = values.Length * 2;
        byte[] frame = new byte[7 + byteCount + 2];
        frame[0] = slaveAddr;
        frame[1] = 0x10;
        frame[2] = (byte)(startAddress >> 8);
        frame[3] = (byte)startAddress;
        frame[4] = (byte)(values.Length >> 8);
        frame[5] = (byte)values.Length;
        frame[6] = (byte)byteCount;

        for (int i = 0; i < values.Length; i++)
        {
            frame[7 + i * 2] = (byte)(values[i] >> 8);
            frame[8 + i * 2] = (byte)values[i];
        }

        byte[] crc = CrcHelper.Crc16ModbusBytes(frame, 0, frame.Length - 2);
        frame[^2] = crc[0];
        frame[^1] = crc[1];
        return frame;
    }

    // === TCP 帧 (MBAP Header) ===

    public static byte[] TcpReadCoils(ushort transactionId, byte unitId, ushort startAddress, ushort quantity)
        => BuildTcpRequest(transactionId, unitId, 0x01, startAddress, quantity);

    public static byte[] TcpReadDiscreteInputs(ushort transactionId, byte unitId, ushort startAddress, ushort quantity)
        => BuildTcpRequest(transactionId, unitId, 0x02, startAddress, quantity);

    public static byte[] TcpReadHoldingRegisters(ushort transactionId, byte unitId, ushort startAddress, ushort quantity)
        => BuildTcpRequest(transactionId, unitId, 0x03, startAddress, quantity);

    public static byte[] TcpReadInputRegisters(ushort transactionId, byte unitId, ushort startAddress, ushort quantity)
        => BuildTcpRequest(transactionId, unitId, 0x04, startAddress, quantity);

    public static byte[] TcpWriteSingleCoil(ushort transactionId, byte unitId, ushort address, bool value)
    {
        ushort coilValue = value ? (ushort)0xFF00 : (ushort)0x0000;
        return BuildTcpRequest(transactionId, unitId, 0x05, address, coilValue);
    }

    public static byte[] TcpWriteSingleRegister(ushort transactionId, byte unitId, ushort address, ushort value)
        => BuildTcpRequest(transactionId, unitId, 0x06, address, value);

    public static byte[] TcpWriteMultipleRegisters(ushort transactionId, byte unitId, ushort startAddress, ushort[] values)
    {
        int byteCount = values.Length * 2;
        int pduLength = 6 + 1 + byteCount; // function + address + quantity + bytecount + data
        byte[] frame = new byte[6 + pduLength]; // MBAP header + PDU

        // MBAP Header
        frame[0] = (byte)(transactionId >> 8);
        frame[1] = (byte)transactionId;
        frame[2] = 0; // Protocol ID
        frame[3] = 0;
        frame[4] = (byte)((pduLength + 1) >> 8);
        frame[5] = (byte)(pduLength + 1); // Unit ID + PDU
        frame[6] = unitId;

        // PDU
        frame[7] = 0x10;
        frame[8] = (byte)(startAddress >> 8);
        frame[9] = (byte)startAddress;
        frame[10] = (byte)(values.Length >> 8);
        frame[11] = (byte)values.Length;
        frame[12] = (byte)byteCount;

        for (int i = 0; i < values.Length; i++)
        {
            frame[13 + i * 2] = (byte)(values[i] >> 8);
            frame[14 + i * 2] = (byte)values[i];
        }

        return frame;
    }

    // === 内部方法 ===

    private static byte[] BuildRtuRequest(byte slaveAddr, byte functionCode, ushort address, ushort value)
    {
        byte[] frame = new byte[8]; // 6 data + 2 CRC
        frame[0] = slaveAddr;
        frame[1] = functionCode;
        frame[2] = (byte)(address >> 8);
        frame[3] = (byte)address;
        frame[4] = (byte)(value >> 8);
        frame[5] = (byte)value;

        byte[] crc = CrcHelper.Crc16ModbusBytes(frame, 0, 6);
        frame[6] = crc[0];
        frame[7] = crc[1];
        return frame;
    }

    private static byte[] BuildTcpRequest(ushort transactionId, byte unitId, byte functionCode, ushort address, ushort value)
    {
        // MBAP Header (7 bytes) + PDU (5 bytes)
        byte[] frame = new byte[12];
        // MBAP Header
        frame[0] = (byte)(transactionId >> 8);
        frame[1] = (byte)transactionId;
        frame[2] = 0; // Protocol ID
        frame[3] = 0;
        frame[4] = 0; // Length
        frame[5] = 6; // Unit ID + PDU (5 bytes)
        frame[6] = unitId;
        // PDU
        frame[7] = functionCode;
        frame[8] = (byte)(address >> 8);
        frame[9] = (byte)address;
        frame[10] = (byte)(value >> 8);
        frame[11] = (byte)value;
        return frame;
    }

    /// <summary>
    /// 获取功能码名称
    /// </summary>
    public static string GetFunctionName(byte functionCode)
    {
        return functionCode switch
        {
            0x01 => "读线圈 (01)",
            0x02 => "读离散输入 (02)",
            0x03 => "读保持寄存器 (03)",
            0x04 => "读输入寄存器 (04)",
            0x05 => "写单个线圈 (05)",
            0x06 => "写单个寄存器 (06)",
            0x0F => "写多个线圈 (15)",
            0x10 => "写多个寄存器 (16)",
            _ => $"未知功能码 (0x{functionCode:X2})"
        };
    }
}
