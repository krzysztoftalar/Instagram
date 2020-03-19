using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class PhotosListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private string Username;

        public PhotosListViewModel(IProfileEndpoint profileEndpoint, IEventAggregator events, IProfile profile)
        {
            _profileEndpoint = profileEndpoint;
            _events = events;
            _profile = profile;

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var username = Username ?? _profile.Username; 
            var profile = await _profileEndpoint.LoadProfile(username);
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
            Username = message.Username;
        }
    }
}
