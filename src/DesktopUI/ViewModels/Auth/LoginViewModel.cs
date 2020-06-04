using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DesktopUI.Commands;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.ViewModels.Auth
{
    public class LoginViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IEventAggregator _events;
        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IUserEndpoint userEndpoint, IEventAggregator events)
        {
            _userEndpoint = userEndpoint;
            _events = events;

            User = new LoginUserFormValues();

            LoginCommand = new LoginCommand(async (parameter) => await LoginAsync(parameter));
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

        private LoginUserFormValues _user;
        public LoginUserFormValues User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
            }
        }

        public async Task LoginAsync(object parameter)
        {
            try
            {
                ErrorMessage = "";

                var authUser = await _userEndpoint.LoginAsync(parameter as LoginUserFormValues);

                await _userEndpoint.CurrentUserAsync(authUser.Token);

                await _events.PublishOnUIThreadAsync(Navigation.Main, new CancellationToken());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async Task GoToRegisterAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Register, new CancellationToken());
        }
    }
}