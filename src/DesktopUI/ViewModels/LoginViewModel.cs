using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using System;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IEventAggregator _events;

        public LoginViewModel(IUserEndpoint userEndpoint, IEventAggregator events)
        {
            _userEndpoint = userEndpoint;
            _events = events;
        }

        private string _email;

        public string Email
        {
            get => _email = "bob@test.com";
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password;

        public string Password
        {
            get => _password = "Pa$$w0rd";
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get
            {
                bool output = Email?.Length > 0 && Password?.Length > 0;
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

        public async Task Login()
        {
            var user = new UserFormValues
            {
                Email = Email,
                Password = Password
            };

            try
            {
                ErrorMessage = "";

                var result = await _userEndpoint.Login(user);

                await _userEndpoint.CurrentUser(result.Token);

                _events.PublishOnUIThread(Navigation.Main);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;

                if (ex.Message == "Unauthorized")
                {
                    ErrorMessage = "Incorrect email address or password, please try again.";
                }
            }
        }

        public void GoToRegister()
        {
            _events.PublishOnUIThread(Navigation.Register);
        }
    }
}