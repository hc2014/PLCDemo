using System;
using System.Net.Sockets;
using System.Threading;

namespace WPFPlcDemo.Model
{
    public class ModbusException : Exception
    {
        public byte FunctionCode { get; }
        public byte ExceptionCode { get; }
        public ModbusException(byte func, byte exCode, string msg) : base(msg)
        {
            FunctionCode = func;
            ExceptionCode = exCode;
        }
    }

    public class ModbusTcpClient : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private ushort _transactionId = 0;
        private readonly object _sync = new object();

        public bool Connect(string host, int port=502)
        {
            try
            {
                _client = new TcpClient(host, port);
                _stream = _client.GetStream();
                return true;
            }
            catch { Disconnect(); return false; }
        }

        public void Disconnect()
        {
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _stream = null;
            _client = null;
        }

        public void Dispose() => Disconnect();

        private byte[] BuildRead03(ushort unitId, ushort startAddress, ushort count)
        {
            byte func = 0x03;
            byte[] pdu = new byte[5];
            pdu[0] = func;
            pdu[1] = (byte)(startAddress >> 8);
            pdu[2] = (byte)(startAddress & 0xFF);
            pdu[3] = (byte)(count >> 8);
            pdu[4] = (byte)(count & 0xFF);
            return SendRequest(unitId, pdu);
        }

        private byte[] SendRequest(ushort unitId, byte[] pdu)
        {
            lock (_sync)
            {
                _transactionId++;
                if (_transactionId == 0) _transactionId = 1;
                ushort len = (ushort)(pdu.Length + 1);
                byte[] mbap = new byte[7];
                mbap[0] = (byte)(_transactionId >> 8);
                mbap[1] = (byte)(_transactionId & 0xFF);
                mbap[2] = 0;
                mbap[3] = 0;
                mbap[4] = (byte)(len >> 8);
                mbap[5] = (byte)(len & 0xFF);
                mbap[6] = (byte)unitId;

                byte[] msg = new byte[7 + pdu.Length];
                Array.Copy(mbap, 0, msg, 0, 7);
                Array.Copy(pdu, 0, msg, 7, pdu.Length);
                _stream.Write(msg,0,msg.Length);

                byte[] header = new byte[7];
                _stream.Read(header,0,7);
                ushort bodyLen = (ushort)((header[4]<<8)|header[5]);
                byte[] body = new byte[bodyLen];
                _stream.Read(body,0,bodyLen);
                if(body[0]==(byte)(pdu[0]+0x80)) throw new ModbusException(body[0], body[1], "Modbus exception");
                return body;
            }
        }

        public ushort[] ReadHoldingRegisters(ushort unitId, ushort startAddress, ushort count)
        {
            var resp = BuildRead03(unitId, startAddress, count);
            ushort[] values = new ushort[count];
            for(int i=0;i<count;i++)
                values[i] = (ushort)((resp[2+i*2]<<8)|resp[3+i*2]);
            return values;
        }

        public void WriteSingleRegister(ushort unitId, ushort address, ushort value)
        {
            byte func = 0x06;
            byte[] pdu = new byte[5];
            pdu[0]=func;
            pdu[1]=(byte)(address>>8);
            pdu[2]=(byte)(address&0xFF);
            pdu[3]=(byte)(value>>8);
            pdu[4]=(byte)(value&0xFF);
            SendRequest(unitId,pdu);
        }

        public void WriteMultipleRegisters(ushort unitId, ushort startAddress, ushort[] values)
        {
            byte func = 0x10;
            byte[] pdu = new byte[6+values.Length*2];
            pdu[0]=func;
            pdu[1]=(byte)(startAddress>>8);
            pdu[2]=(byte)(startAddress&0xFF);
            pdu[3]=(byte)(values.Length>>8);
            pdu[4]=(byte)(values.Length&0xFF);
            pdu[5]=(byte)(values.Length*2);
            for(int i=0;i<values.Length;i++)
            {
                pdu[6+i*2]=(byte)(values[i]>>8);
                pdu[7+i*2]=(byte)(values[i]&0xFF);
            }
            SendRequest(unitId,pdu);
        }
    }
}
