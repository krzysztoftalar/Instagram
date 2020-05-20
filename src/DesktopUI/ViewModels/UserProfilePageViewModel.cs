using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DesktopUI.ViewModels
{
    public class UserProfilePageViewModel : Conductor<object>, IHandle<MessageEvent>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IMessage _messageEvent;
        private string _username;

        public UserProfilePageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profileEndpoint, IProfile profile, IMessage messageEvent)
        {
            _events = events;
            _user = user;
            _profileEndpoint = profileEndpoint;
            _profile = profile;
            _messageEvent = messageEvent;

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var profile = await _profileEndpoint.LoadProfile(_username);
            FollowersCount = profile.FollowersCount.ToString();
            FollowingCount = profile.FollowingCount.ToString();

            NotifyOfPropertyChange(() => DisplayName);
            NotifyOfPropertyChange(() => Image);
            NotifyOfPropertyChange(() => FollowBtnContent);
            NotifyOfPropertyChange(() => FollowBtnBorder);
        }

        public bool IsLogIn => _user.Username == _username;

        public bool IsCurrentUser => _user.Username != _username;

        private string _followersCount;

        public string FollowersCount
        {
            get => _followersCount = _profile.FollowersCount.ToString();
            set
            {
                _followersCount = value;
                NotifyOfPropertyChange(() => FollowersCount);
            }
        }

        private string _followingCount;

        public string FollowingCount
        {
            get => _followingCount;
            set
            {
                _followingCount = value;
                NotifyOfPropertyChange(() => FollowingCount);
            }
        }

        private string _followBtnContent;

        public string FollowBtnContent
        {
            get => _profile.Following ? "UNFOLLOW" : "FOLLOW";
            set
            {
                _followBtnContent = value;
                NotifyOfPropertyChange(() => FollowBtnContent);
            }
        }

        private SolidColorBrush _followBtnBorder;

        public SolidColorBrush FollowBtnBorder
        {
            get => _profile.Following ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
            set
            {
                _followBtnBorder = value;
                NotifyOfPropertyChange(() => FollowBtnBorder);
            }
        }

        public async Task Follow()
        {
            if (_profile.Following)
            {
                await _profileEndpoint.UnFollow(_profile.Username);
            }
            else
            {
                await _profileEndpoint.Follow(_profile.Username);
            }

            NotifyOfPropertyChange(() => FollowBtnContent);
            NotifyOfPropertyChange(() => FollowBtnBorder);
            NotifyOfPropertyChange(() => FollowersCount);
        }

        private string _displayName;

        public new string DisplayName
        {
            get => _displayName = _profile.DisplayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        private string _image;

        public string Image
        {
            get => _image = _profile.Image ?? "../Assets/user.png";
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        public void UploadPhoto()
        {
            ActivateItem(IoC.Get<AddPhotoViewModel>());
        }

        public void EditProfile()
        {
            ActivateItem(IoC.Get<EditProfileViewModel>());

            _events.PublishOnUIThread(new MessageEvent { DisplayName = _profile.DisplayName, Bio = _profile.Bio });
        }

        public void PhotosList()
        {
            ActivateItem(IoC.Get<PhotosListViewModel>());

            _events.PublishOnUIThread(new ModeEvent { IsEditMode = true });

            _messageEvent.OnProfilePage(true);
        }

        public void LoadFollowing()
        {
            ActivateItem(IoC.Get<FollowersListViewModel>());

            _events.PublishOnUIThread(new MessageEvent { Predicate = "following" });
        }

        public void LoadFollowers()
        {
            ActivateItem(IoC.Get<FollowersListViewModel>());

            _events.PublishOnUIThread(new MessageEvent { Predicate = "followers" });
        }

        public void BackToMainPage()
        {
            _events.PublishOnUIThread(Navigation.Main);

            _events.PublishOnUIThread(new ModeEvent { IsEditMode = false });
        }

        public void Handle(MessageEvent message)
        {
            _username = message.Username;

            NotifyOfPropertyChange(() => Image);
            NotifyOfPropertyChange(() => DisplayName);
        }
    }
}