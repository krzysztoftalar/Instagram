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
            get => _email;
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
            get => _password;
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

        public async Task Login()
        {
            var user = new UserFormValues
            {
                Email = Email,
                Password = Password
            };

            try
            {
                await _userEndpoint.Login(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GoToRegister()
        {
            _events.PublishOnUIThread(new RegisterEvent());
        }
    }
}
