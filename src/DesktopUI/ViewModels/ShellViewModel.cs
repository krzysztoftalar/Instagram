using Caliburn.Micro;
using DesktopUI.EventModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<Navigation>
    {
        public ShellViewModel(IEventAggregator events)
        {
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());

            events.SubscribeOnPublishedThread(this);
        }

        public sealed override Task ActivateItemAsync(object item, CancellationToken cancellationToken)
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        public async Task HandleAsync(Navigation message, CancellationToken cancellationToken)
        {
            switch (message)
            {
                case Navigation.Login:
                    await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
                    break;

                case Navigation.Register:
                    await ActivateItemAsync(IoC.Get<RegisterViewModel>(), new CancellationToken());
                    break;

                case Navigation.Main:
                    await ActivateItemAsync(IoC.Get<UserMainPageViewModel>(), new CancellationToken());
                    break;

                case Navigation.Profile:
                    await ActivateItemAsync(IoC.Get<UserProfilePageViewModel>(), new CancellationToken());
                    break;

                case Navigation.Chat:
                    await ActivateItemAsync(IoC.Get<ChatPageViewModel>(), new CancellationToken());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }
        }
    }
}

