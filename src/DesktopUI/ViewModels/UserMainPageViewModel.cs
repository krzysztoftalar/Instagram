using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class UserMainPageViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser user)
        {
            _events = events;
            _user = user;
        }

        protected override void OnViewLoaded(object view)
        {
            Image = _user.Image;
        }

        private string _image;

        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }


        public void EditProfile()
        {
            _events.PublishOnUIThread(ProjectSignals.Profile);
        }
    }
}
