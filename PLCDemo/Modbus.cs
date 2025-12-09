using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCDemo
{
    public static class Modbus
    { 
        //============================
        // 标准 CRC16（Modbus）
        //============================
        public static byte[] CRC16(byte[] data)
        {
            ushort crc = 0xFFFF;

            foreach (byte b in data)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return new byte[]
            {
                (byte)(crc & 0xFF),        // 低位
                (byte)((crc >> 8) & 0xFF)  // 高位
            };
        }

        //============================
        // 构造 03 功能码（读保持寄存器）
        //============================
        public static byte[] BuildRead03(byte slave, ushort addr, ushort len)
        {
            byte[] frame = new byte[6];

            frame[0] = slave;
            frame[1] = 0x03;
            frame[2] = (byte)(addr >> 8);
            frame[3] = (byte)(addr & 0xFF);
            frame[4] = (byte)(len >> 8);
            frame[5] = (byte)(len & 0xFF);

            byte[] crc = CRC16(frame);

            byte[] full = new byte[frame.Length + 2];
            Array.Copy(frame, full, frame.Length);
            full[6] = crc[0];
            full[7] = crc[1];

            return full;
        }

        //============================
        // 构造 06 功能码（写单个寄存器）
        //============================
        public static byte[] BuildWrite06(byte slave, ushort addr, ushort value)
        {
            byte[] frame = new byte[6];

            frame[0] = slave;
            frame[1] = 0x06;
            frame[2] = (byte)(addr >> 8);
            frame[3] = (byte)(addr & 0xFF);
            frame[4] = (byte)(value >> 8);
            frame[5] = (byte)(value & 0xFF);

            byte[] crc = CRC16(frame);

            byte[] full = new byte[frame.Length + 2];
            Array.Copy(frame, full, frame.Length);
            full[6] = crc[0];
            full[7] = crc[1];

            return full;
        }
    }
}
