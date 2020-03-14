using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profiles;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class PhotosListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profile;
        private readonly IEventAggregator _events;
        private string Username;

        public PhotosListViewModel(IProfileEndpoint profile, IEventAggregator events)
        {
            _profile = profile;
            _events = events;

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var profile = await _profile.LoadProfile(Username);
            UserPhotos = new BindableCollection<Photo>(profile.Photos);
        }

        private BindableCollection<Photo> _userPhotos;

        public BindableCollection<Photo> UserPhotos
        {
            get => _userPhotos;
            set
            {
                _userPhotos = value;
                NotifyOfPropertyChange(() => UserPhotos);
            }
        }

        public void Handle(MessageEvent message)
        {
            Username = message.Message;
        }
    }
}
