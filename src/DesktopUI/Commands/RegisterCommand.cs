using System;
using System.Windows.Input;
using DesktopUI.Library.Models;

namespace DesktopUI.Commands
{
    public class RegisterCommand : ICommand
    {
        private readonly Action<object> _action;

        public RegisterCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            var user = parameter as RegisterUserFormValues;

            return !string.IsNullOrWhiteSpace(user?.Email) && !string.IsNullOrWhiteSpace(user?.Password) &&
                   !string.IsNullOrWhiteSpace(user?.DisplayName) && !string.IsNullOrWhiteSpace(user?.Username);
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