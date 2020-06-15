using Caliburn.Micro;
using DesktopUI.Commands;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using DesktopUI.ViewModels.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopUI.ViewModels.Auth
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IEventAggregator _events;

        public RegisterViewModel(IUserEndpoint userEndpoint, IEventAggregator events)
        {
            _userEndpoint = userEndpoint;
            _events = events;

            User = new RegisterUserFormValues();
        }

        private ICommand _registerCommand;

        public ICommand RegisterCommand => _registerCommand ??= new RelayParameterizedCommand<RegisterUserFormValues>(
            async user => await RegisterAsync(user), CanRegister);

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

        private bool _registerIsRunning;

        public bool RegisterIsRunning
        {
            get => _registerIsRunning;
            set
            {
                _registerIsRunning = value;
                NotifyOfPropertyChange(() => RegisterIsRunning);
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

        public bool CanRegister(RegisterUserFormValues user) => user?.IsValid ?? false;

        public async Task RegisterAsync(RegisterUserFormValues user)
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                try
                {
                    ErrorMessage = "";

                    await _userEndpoint.RegisterAsync(user);

                    MessageBox.Show("You have been successfully registered. We have sent an email with a confirmation link to your email address. In order to complete the sign-up process, please click the confirmation link.", "Congratulations!",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    await _events.PublishOnUIThreadAsync(Navigation.Login, new CancellationToken());
                    await _events.PublishOnUIThreadAsync(this, new CancellationToken());
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            });
        }

        public async Task GoToLoginAsync()
        {
            await Task.Run(async () =>
            {
                await _events.PublishOnUIThreadAsync(Navigation.Login, new CancellationToken());
                await Task.Delay(900);
                await _events.PublishOnUIThreadAsync(this, new CancellationToken());
            });
        }
    }
}