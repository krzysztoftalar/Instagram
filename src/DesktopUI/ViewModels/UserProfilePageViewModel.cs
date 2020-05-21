using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using System.Threading;
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
        private string _username;

        public UserProfilePageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profileEndpoint, IProfile profile)
        {
            _events = events;
            _user = user;
            _profileEndpoint = profileEndpoint;
            _profile = profile;

            _events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var profile = await _profileEndpoint.LoadProfileAsync(_username);
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

        public async Task FollowAsync()
        {
            if (_profile.Following)
            {
                await _profileEndpoint.UnFollowAsync(_profile.Username);
            }
            else
            {
                await _profileEndpoint.FollowAsync(_profile.Username);
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

        public async Task UploadPhoto()
        {
            await ActivateItemAsync(IoC.Get<AddPhotoViewModel>(), new CancellationToken());
        }

        public async Task EditProfileAsync()
        {
            await ActivateItemAsync(IoC.Get<EditProfileViewModel>(), new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent
            {
                DisplayName = _profile.DisplayName,
                Bio = _profile.Bio
            }, new CancellationToken());
        }

        public async Task PhotosListAsync()
        {
            await ActivateItemAsync(IoC.Get<PhotosListViewModel>(), new CancellationToken());

            await _events.PublishOnUIThreadAsync(new ModeEvent
            {
                IsEditMode = true,
            }, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new NavigationEvent
            {
                IsProfilePageActive = true
            }, new CancellationToken());
        }

        public async Task LoadFollowingAsync()
        {
            await ActivateItemAsync(IoC.Get<FollowersListViewModel>(), new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent
            {
                Predicate = "following"
            }, new CancellationToken());
        }

        public async Task LoadFollowersAsync()
        {
            await ActivateItemAsync(IoC.Get<FollowersListViewModel>(), new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent
            {
                Predicate = "followers"
            }, new CancellationToken());
        }

        public async Task BackToMainPageAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Main, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new ModeEvent
            {
                IsEditMode = false
            }, new CancellationToken());
        }

        public async Task HandleAsync(MessageEvent message, CancellationToken cancellationToken)
        {
            await Task.FromResult(_username = message.Username);

            NotifyOfPropertyChange(() => Image);
            NotifyOfPropertyChange(() => DisplayName);
        }
    }
}