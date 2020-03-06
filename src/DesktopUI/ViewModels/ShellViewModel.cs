using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<Navigation>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;

        public ShellViewModel(IEventAggregator events, IAuthenticatedUser user)
        {
            ActivateItem(IoC.Get<LoginViewModel>());

            _events = events;
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

        //public bool IsLoggedIn
        //{
        //    get
        //    {
        //        bool output = string.IsNullOrWhiteSpace(_user.Token) == false;

        //        return output;
        //    }
        //}

        public void Handle(Navigation message)
        {
            switch (message)
            {
                case Navigation.Login:
                    ActivateItem(IoC.Get<LoginViewModel>());
                    break;

                case Navigation.Register:
                    ActivateItem(IoC.Get<RegisterViewModel>());
                    break;

                case Navigation.Main:
                    ActivateItem(IoC.Get<UserMainPageViewModel>());
                    //NotifyOfPropertyChange(() => IsLoggedIn);
                    break;

                case Navigation.Profile:
                    ActivateItem(IoC.Get<UserProfilePageViewModel>());
                    break;
            }
        }
    }
}

