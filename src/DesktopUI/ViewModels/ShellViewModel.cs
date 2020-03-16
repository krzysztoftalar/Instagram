using Caliburn.Micro;
using DesktopUI.EventModels;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<Navigation>
    {
        private readonly IEventAggregator _events;

        public ShellViewModel(IEventAggregator events)
        {
            ActivateItem(IoC.Get<LoginViewModel>());

            _events = events;

            _events.Subscribe(this);
        }

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
                    break;

                case Navigation.Profile:
                    ActivateItem(IoC.Get<UserProfilePageViewModel>());
                    break;
            }
        }
    }
}

