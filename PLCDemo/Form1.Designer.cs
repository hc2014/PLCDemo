namespace PLCDemo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboPort = new System.Windows.Forms.ComboBox();
            this.comboBaud = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtSlave = new System.Windows.Forms.TextBox();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.btnRead03 = new System.Windows.Forms.Button();
            this.txtWriteValue = new System.Windows.Forms.TextBox();
            this.btnWrite06 = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboPort
            // 
            this.comboPort.Location = new System.Drawing.Point(12, 12);
            this.comboPort.Name = "comboPort";
            this.comboPort.Size = new System.Drawing.Size(121, 20);
            this.comboPort.TabIndex = 0;
            // 
            // comboBaud
            // 
            this.comboBaud.Location = new System.Drawing.Point(150, 12);
            this.comboBaud.Name = "comboBaud";
            this.comboBaud.Size = new System.Drawing.Size(91, 20);
            this.comboBaud.TabIndex = 1;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(270, 9);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开串口";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtSlave
            // 
            this.txtSlave.Location = new System.Drawing.Point(12, 60);
            this.txtSlave.Name = "txtSlave";
            this.txtSlave.Size = new System.Drawing.Size(100, 21);
            this.txtSlave.TabIndex = 3;
            this.txtSlave.Text = "1";
            // 
            // txtAddr
            // 
            this.txtAddr.Location = new System.Drawing.Point(118, 60);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(62, 21);
            this.txtAddr.TabIndex = 4;
            this.txtAddr.Text = "0";
            // 
            // txtLen
            // 
            this.txtLen.Location = new System.Drawing.Point(186, 60);
            this.txtLen.Name = "txtLen";
            this.txtLen.Size = new System.Drawing.Size(64, 21);
            this.txtLen.TabIndex = 5;
            this.txtLen.Text = "1";
            // 
            // btnRead03
            // 
            this.btnRead03.Location = new System.Drawing.Point(270, 60);
            this.btnRead03.Name = "btnRead03";
            this.btnRead03.Size = new System.Drawing.Size(75, 23);
            this.btnRead03.TabIndex = 6;
            this.btnRead03.Text = "读保持寄存器(03)";
            this.btnRead03.Click += new System.EventHandler(this.btnRead03_Click);
            // 
            // txtWriteValue
            // 
            this.txtWriteValue.Location = new System.Drawing.Point(12, 100);
            this.txtWriteValue.Name = "txtWriteValue";
            this.txtWriteValue.Size = new System.Drawing.Size(100, 21);
            this.txtWriteValue.TabIndex = 7;
            this.txtWriteValue.Text = "100";
            // 
            // btnWrite06
            // 
            this.btnWrite06.Location = new System.Drawing.Point(136, 100);
            this.btnWrite06.Name = "btnWrite06";
            this.btnWrite06.Size = new System.Drawing.Size(75, 23);
            this.btnWrite06.TabIndex = 8;
            this.btnWrite06.Text = "写单寄存器(06)";
            this.btnWrite06.Click += new System.EventHandler(this.btnWrite06_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 150);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(360, 220);
            this.txtLog.TabIndex = 9;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.comboPort);
            this.Controls.Add(this.comboBaud);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.txtSlave);
            this.Controls.Add(this.txtAddr);
            this.Controls.Add(this.txtLen);
            this.Controls.Add(this.btnRead03);
            this.Controls.Add(this.txtWriteValue);
            this.Controls.Add(this.btnWrite06);
            this.Controls.Add(this.txtLog);
            this.Name = "Form1";
            this.Text = "Modbus RTU Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboPort;
        private System.Windows.Forms.ComboBox comboBaud;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txtSlave;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.TextBox txtLen;
        private System.Windows.Forms.Button btnRead03;
        private System.Windows.Forms.TextBox txtWriteValue;
        private System.Windows.Forms.Button btnWrite06;
        private System.Windows.Forms.TextBox txtLog;
    }
}
