using System;
using System.Windows.Input;

namespace WPFPlcDemo.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object,bool> _can;
        public RelayCommand(Action<object> execute, Func<object,bool> can=null) { _execute=execute; _can=can; }
        public bool CanExecute(object parameter) => _can==null||_can(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged;
    }
}
