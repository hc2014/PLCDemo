using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFPlcDemo.Model;

namespace WPFPlcDemo.ViewModel
{
    public class RegisterItem : INotifyPropertyChanged
    {
        public int Address {get;set;}
        private ushort _value;
        public ushort Value {get=>_value; set{_value=value; OnPropertyChanged();}}
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string p=null)=>PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(p));
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RegisterItem> Registers {get;} = new ObservableCollection<RegisterItem>();
        private ModbusTcpClient _client = new ModbusTcpClient();
        public bool Connected {get; private set;}

        public ICommand ConnectCommand {get;}
        public ICommand WriteSingleCommand {get;}
        public ICommand WriteMultipleCommand {get;}

        public MainViewModel()
        {
            for(int i=0;i<10;i++) Registers.Add(new RegisterItem{Address=i, Value=0});
            ConnectCommand = new RelayCommand(async _=> await ConnectAsync());
            WriteSingleCommand = new RelayCommand(async p=> await WriteSingleAsync(p));
            WriteMultipleCommand = new RelayCommand(async _=> await WriteMultipleAsync());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Raise([CallerMemberName]string p=null)=>PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(p));

        private Task ConnectAsync()
        {
            return Task.Run(()=>{
                Connected = _client.Connect("127.0.0.1",502);
                Raise(nameof(Connected));
                if(Connected)
                {
                    Task.Run(async ()=>{
                        while(Connected)
                        {
                            try{
                                var vals=_client.ReadHoldingRegisters(1,0,(ushort)Registers.Count);
                                Application.Current.Dispatcher.Invoke(()=>{
                                    for(int i=0;i<Registers.Count;i++) Registers[i].Value=vals[i];
                                });
                            }catch{}
                            await Task.Delay(500);
                        }
                    });
                }
            });
        }

        private Task WriteSingleAsync(object param)
        {
            return Task.Run(()=>{
                if(param is RegisterItem ri)
                {
                    try{ _client.WriteSingleRegister(1,(ushort)ri.Address,ri.Value); } 
                    catch(Exception ex){ Application.Current.Dispatcher.Invoke(()=>MessageBox.Show("Write error: "+ex.Message)); }
                }
            });
        }

        private Task WriteMultipleAsync()
        {
            return Task.Run(()=>{
                try{
                    ushort[] vals = new ushort[5];
                    for(int i=0;i<5;i++) vals[i]=Registers[i].Value;
                    _client.WriteMultipleRegisters(1,0,vals);
                }catch(Exception ex){ Application.Current.Dispatcher.Invoke(()=>MessageBox.Show("Write multiple error: "+ex.Message));}
            });
        }
    }
}
