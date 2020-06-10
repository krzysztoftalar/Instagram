using Caliburn.Micro;
using DesktopUI.Commands;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using DesktopUI.ViewModels.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopUI.ViewModels.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IEventAggregator _events;

        public LoginViewModel(IUserEndpoint userEndpoint, IEventAggregator events)
        {
            _userEndpoint = userEndpoint;
            _events = events;

            User = new LoginUserFormValues();
        }

        private ICommand _loginCommand;

        public ICommand LoginCommand => _loginCommand ??= new RelayParameterizedCommand<LoginUserFormValues>(
          async user => await LoginAsync(user), CanLoginAsync);

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

        private bool _loginIsRunning;

        public bool LoginIsRunning
        {
            get => _loginIsRunning;
            set
            {
                _loginIsRunning = value;
                NotifyOfPropertyChange(() => LoginIsRunning);
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

        public bool CanLoginAsync(LoginUserFormValues user) =>
            !string.IsNullOrWhiteSpace(user?.Email) && !string.IsNullOrWhiteSpace(user?.Password);

        public async Task LoginAsync(LoginUserFormValues user)
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                try
                {
                    ErrorMessage = "";

                    var authUser = await _userEndpoint.LoginAsync(user);

                    await _userEndpoint.CurrentUserAsync(authUser.Token);

                    await _events.PublishOnUIThreadAsync(Navigation.Main, new CancellationToken());
                    await _events.PublishOnUIThreadAsync(this, new CancellationToken());
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            });
        }

        public async Task GoToRegisterAsync()
        {
            await Task.Run(async () =>
            {
                await _events.PublishOnUIThreadAsync(Navigation.Register, new CancellationToken());
                await Task.Delay(900);
                await _events.PublishOnUIThreadAsync(this, new CancellationToken());
            });
        }
    }
}