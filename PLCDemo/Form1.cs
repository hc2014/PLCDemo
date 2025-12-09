using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLCDemo
{
    public partial class Form1 : Form
    {
        private SerialPort _serial;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboPort.Items.AddRange(SerialPort.GetPortNames());
            comboPort.SelectedIndex = comboPort.Items.Count > 0 ? 0 : -1;

            comboBaud.Items.Add("9600");
            comboBaud.Items.Add("19200");
            comboBaud.Items.Add("38400");
            comboBaud.Items.Add("115200");
            comboBaud.SelectedIndex = 0;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                _serial = new SerialPort(comboPort.Text, int.Parse(comboBaud.Text), Parity.None, 8, StopBits.One);
                _serial.Open();
                MessageBox.Show("串口已打开");
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开失败: " + ex.Message);
            }
        }

        //=====================================
        // 读取 03 功能码（保持寄存器）
        //=====================================
        private void btnRead03_Click(object sender, EventArgs e)
        {
            byte slave = byte.Parse(txtSlave.Text);
            ushort addr = ushort.Parse(txtAddr.Text);
            ushort len = ushort.Parse(txtLen.Text);

            byte[] frame = Modbus.BuildRead03(slave, addr, len);

            byte[] resp = SendAndReceive(frame);
            if (resp == null) return;

            txtLog.AppendText("读结果: " + BitConverter.ToString(resp) + Environment.NewLine);
        }

        //=====================================
        // 写 06 功能码（写单个寄存器）
        //=====================================
        private void btnWrite06_Click(object sender, EventArgs e)
        {
            byte slave = byte.Parse(txtSlave.Text);
            ushort addr = ushort.Parse(txtAddr.Text);
            ushort value = ushort.Parse(txtWriteValue.Text);

            byte[] frame = Modbus.BuildWrite06(slave, addr, value);

            byte[] resp = SendAndReceive(frame);
            if (resp == null) return;

            txtLog.AppendText("写结果: " + BitConverter.ToString(resp) + Environment.NewLine);
        }

        //=====================================
        // 发送并接收 Modbus RTU 数据
        //=====================================
        private byte[] SendAndReceive(byte[] frame)
        {
            if (_serial == null || !_serial.IsOpen)
            {
                MessageBox.Show("串口未打开");
                return null;
            }

            txtLog.AppendText("发送: " + BitConverter.ToString(frame) + Environment.NewLine);

            _serial.DiscardInBuffer();
            _serial.Write(frame, 0, frame.Length);

            System.Threading.Thread.Sleep(150);

            int count = _serial.BytesToRead;
            byte[] buffer = new byte[count];
            _serial.Read(buffer, 0, count);

            txtLog.AppendText("接收: " + BitConverter.ToString(buffer) + Environment.NewLine);
            return buffer;
        }
    }
}
