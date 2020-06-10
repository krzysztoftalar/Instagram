using System;
using System.Windows.Input;

namespace DesktopUI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecuteAction;

        public RelayCommand(Action action, Func<bool> canExecuteAction = null)
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
            return _canExecuteAction == null || _canExecuteAction();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}