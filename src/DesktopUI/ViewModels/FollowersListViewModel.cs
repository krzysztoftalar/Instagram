using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profiles;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class FollowersListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IEventAggregator _events;
        private string _predicate;

        public FollowersListViewModel(IProfileEndpoint profileEndpoint, IProfile profile, IEventAggregator events)
        {
            _profileEndpoint = profileEndpoint;
            _profile = profile;
            _events = events;

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var profiles = await _profileEndpoint.LoadFollowing(_profile.Username, _predicate);
            UserFollowing = new BindableCollection<Profile>(profiles);
        }

        private BindableCollection<Profile> _userFollowing;

        public BindableCollection<Profile> UserFollowing
        {
            get => _userFollowing;
            set
            {
                _userFollowing = value;
                NotifyOfPropertyChange(() => UserFollowing);
            }
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

        public void Handle(MessageEvent message)
        {
            _predicate = message.Message;
        }
    }
}
