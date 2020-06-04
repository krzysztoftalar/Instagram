using System;
using System.Windows.Input;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly Action<object> _action;

        public LoginCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            var user = parameter as LoginUserFormValues;

            return !string.IsNullOrWhiteSpace(user?.Email) && !string.IsNullOrWhiteSpace(user?.Password);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}