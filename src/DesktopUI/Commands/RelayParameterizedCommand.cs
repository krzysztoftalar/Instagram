using System;
using System.Windows.Input;

namespace DesktopUI.Commands
{
    public class RelayParameterizedCommand<T> : ICommand
    {
        private readonly Action<T> _action;
        private readonly Func<T, bool> _canExecuteAction;

        public RelayParameterizedCommand(Action<T> action, Func<T, bool> canExecuteAction = null)
        {
            _action = action;
            _canExecuteAction = canExecuteAction;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null || _canExecuteAction((T)parameter);
        }

        public void Execute(object parameter)
        {
            _action((T)parameter);
        }
    }
}