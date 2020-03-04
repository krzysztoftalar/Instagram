using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<ProjectSignals>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IUserEndpoint _userEndpoint;

        public ShellViewModel(IEventAggregator events, IAuthenticatedUser user, IUserEndpoint userEndpoint)
        {
            ActivateItem(IoC.Get<LoginViewModel>());

            _events = events;
            _userEndpoint = userEndpoint;
            _user = user;

            _events.Subscribe(this);
        }

        //protected override async void OnViewLoaded(object view)
        //{
        //    try
        //    {
        //        await _userEndpoint.CurrentUser(_user.Token);
        //    }
        //    catch
        //    {

        //    }
        //}

        public bool IsLoggedIn
        {
            get
            {
                bool output = string.IsNullOrWhiteSpace(_user.Token) == false;

                return output;
            }
        }

        public void Handle(ProjectSignals message)
        {
            switch (message)
            {
                case ProjectSignals.Login:
                    ActivateItem(IoC.Get<LoginViewModel>());
                    break;

                case ProjectSignals.Register:
                    ActivateItem(IoC.Get<RegisterViewModel>());
                    break;

                case ProjectSignals.Authenticated:
                    ActivateItem(IoC.Get<UserMainPageViewModel>());
                    NotifyOfPropertyChange(() => IsLoggedIn);
                    break;

                case ProjectSignals.AddPhoto:
                    ActivateItem(IoC.Get<AddPhotoViewModel>());
                    break;

                case ProjectSignals.Profile:
                    ActivateItem(IoC.Get<UserProfilePageViewModel>());
                    break;
            }
        }
    }
}

