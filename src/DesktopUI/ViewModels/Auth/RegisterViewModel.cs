using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using DesktopUI.Commands;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using DesktopUI.Validators;

namespace DesktopUI.ViewModels.Auth
{
    public class RegisterViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IEventAggregator _events;
        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel(IUserEndpoint userEndpoint, IEventAggregator events)
        {
            _userEndpoint = userEndpoint;
            _events = events;

            User = new RegisterUserFormValues();

            RegisterCommand = new RegisterCommand(async (parameter) => await RegisterAsync(parameter));
        }

        public bool IsErrorVisible => ErrorMessage?.Length > 0;

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        private RegisterUserFormValues _user;
        public RegisterUserFormValues User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
            }
        }

        public async Task RegisterAsync(object parameter)
        {
            if (parameter is RegisterUserFormValues user &&
                (user.Password.IsValidPassword(ref _errorMessage) &&
                 user.Email.IsValidEmail(ref _errorMessage)))
            {
                try
                {
                    await _userEndpoint.RegisterAsync(user);

                    MessageBox.Show("You have been successfully registered.", "Congratulations!",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    await _events.PublishOnUIThreadAsync(Navigation.Login, new CancellationToken());
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }

            NotifyOfPropertyChange(() => ErrorMessage);
            NotifyOfPropertyChange(() => IsErrorVisible);
        }

        public async Task GoToLoginAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Login, new CancellationToken());
        }
    }
}