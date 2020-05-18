using Caliburn.Micro;
using DesktopUI.EventModels;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<Navigation>
    {
        public ShellViewModel(IEventAggregator events)
        {
            ActivateItem(IoC.Get<LoginViewModel>());

            events.Subscribe(this);
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

                case Navigation.Chat:
                    ActivateItem(IoC.Get<ChatPageViewModel>());
                    break;
            }
        }
    }
}

