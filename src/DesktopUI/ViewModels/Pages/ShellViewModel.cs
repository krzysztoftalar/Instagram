using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.ViewModels.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels.Pages
{
    public class ShellViewModel : Conductor<Screen>.Collection.AllActive, IHandle<Navigation>, IHandle<Screen>
    {
        public ShellViewModel(IEventAggregator events)
        {
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());

            events.SubscribeOnPublishedThread(this);
        }

        public sealed override Task ActivateItemAsync(Screen item, CancellationToken cancellationToken)
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        public async Task HandleAsync(Navigation message, CancellationToken cancellationToken)
        {
            switch (message)
            {
                case Navigation.Login:
                    await ActivateItemAsync(IoC.Get<LoginViewModel>(), cancellationToken);
                    break;

                case Navigation.Register:
                    await ActivateItemAsync(IoC.Get<RegisterViewModel>(), cancellationToken);
                    break;

                case Navigation.Main:
                    await ActivateItemAsync(IoC.Get<UserMainPageViewModel>(), cancellationToken);
                    break;

                case Navigation.Profile:
                    await ActivateItemAsync(IoC.Get<UserProfilePageViewModel>(), cancellationToken);
                    break;

                case Navigation.Chat:
                    await ActivateItemAsync(IoC.Get<ChatPageViewModel>(), cancellationToken);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }
        }

        public async Task HandleAsync(Screen message, CancellationToken cancellationToken)
        {
            await DeactivateItemAsync(message, true, cancellationToken);
            await DeactivateItemAsync(message.Parent as Screen, true, cancellationToken);
        }
    }
}