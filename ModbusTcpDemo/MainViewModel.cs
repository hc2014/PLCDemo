using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ModbusTcpDemo
{
   public class MainViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void Raise([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private readonly ModbusTcpClient _modbus = new ModbusTcpClient();
        private readonly Timer _pollTimer;

        private ushort _reg0;
        public ushort Reg0
        {
            get => _reg0;
            set { _reg0 = value; Raise(); }
        }

        private ushort _reg1;
        public ushort Reg1
        {
            get => _reg1;
            set { _reg1 = value; Raise(); }
        }

        public RelayCommand ConnectCommand { get; }

        public MainViewModel()
        {
            ConnectCommand = new RelayCommand(() =>
            {
                if (_modbus.Connect("127.0.0.1", 502))
                {
                    StartPolling();
                }
            });

            _pollTimer = new Timer(500);
            _pollTimer.Elapsed += Poll;
        }

        private void StartPolling()
        {
            _pollTimer.Start();
        }

        private void Poll(object sender, ElapsedEventArgs e)
        {
            try
            {
                var values = _modbus.ReadHoldingRegisters(0, 2);
                Reg0 = values[0];
                Reg1 = values[1];
            }
            catch { }
        }
    }
}
