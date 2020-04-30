using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopUI.ViewModels
{
    public class RegisterViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IEventAggregator _events;

        public RegisterViewModel(IUserEndpoint userEndpoint, IEventAggregator events)
        {
            _userEndpoint = userEndpoint;
            _events = events;
        }

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        private string _displayName;

        public new string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public bool CanRegister
        {
            get
            {
                bool output = Username?.Length > 0 && DisplayName?.Length > 0 &&
                    Email?.Length > 0 && Password?.Length > 0;
                return output;
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = ErrorMessage?.Length > 0;
                return output;
            }
        }

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

        public async Task Register()
        {
            var user = new UserFormValues
            {
                Username = Username,
                DisplayName = DisplayName,
                Email = Email,
                Password = Password
            };

            try
            {
                await _userEndpoint.Register(user);

                MessageBox.Show("You have been successfully registered.", "Congratulations!",
                     MessageBoxButton.OK, MessageBoxImage.Information);

                _events.PublishOnUIThread(Navigation.Login);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void GoToLogin()
        {
            _events.PublishOnUIThread(Navigation.Login);
        }
    }
}
