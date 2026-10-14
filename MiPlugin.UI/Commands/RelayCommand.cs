using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiPlugin.UI.Commands
{
    // permite que un boton de la ventana ejecute un metodo de ViewModel
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;      //el metodo a usar
            _canExecute = canExecute;   // dice si esta activo
        }
        //WPF pregunta si esta activo y _canExecute responde 
        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;


        //al pulsar el boton ejecuta _execute
        public void Execute(object? parameter) => _execute();


        //hace que WPF vuelva a preguntar cada vez que hagas click o presiones una tecla
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
