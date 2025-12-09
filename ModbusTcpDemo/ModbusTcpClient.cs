using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTcpDemo
{
    public class ModbusTcpClient
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private ushort _transactionId = 0;

        public bool Connect(string ip, int port)
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(ip, port);
                _stream = _client.GetStream();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
        }

        // 读取保持寄存器（FC=03）
        public ushort[] ReadHoldingRegisters(ushort startAddr, ushort count)
        {
            _transactionId++;

            byte[] frame = new byte[12];
            frame[0] = (byte)(_transactionId >> 8);
            frame[1] = (byte)(_transactionId);
            frame[2] = 0;
            frame[3] = 0;

            frame[4] = 0;
            frame[5] = 6;

            frame[6] = 1;
            frame[7] = 3;

            frame[8] = (byte)(startAddr >> 8);
            frame[9] = (byte)(startAddr);

            frame[10] = (byte)(count >> 8);
            frame[11] = (byte)(count);

            _stream.Write(frame, 0, frame.Length);

            byte[] response = new byte[256];
            int len = _stream.Read(response, 0, response.Length);

            ushort[] values = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = (ushort)((response[9 + i * 2] << 8) | response[10 + i * 2]);
            }

            return values;
        }
    }
}
