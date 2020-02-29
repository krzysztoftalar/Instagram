using Caliburn.Micro;
using DesktopUI.EventModels;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<RegisterEvent>
    {
        private readonly IEventAggregator _events;
        private readonly LoginViewModel _loginVM;
        private readonly RegisterViewModel _registerVM;

        public ShellViewModel(IEventAggregator events, LoginViewModel loginVM, RegisterViewModel registerVM)
        {
            ActivateItem(IoC.Get<UserMainPageViewModel>());

            _events = events;
            _loginVM = loginVM;
            _registerVM = registerVM;

            _events.Subscribe(this);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_loginVM);
        }

        public void Handle(RegisterEvent message)
        {
            ActivateItem(_registerVM);
        }
    }
}
