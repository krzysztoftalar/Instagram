using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class UserProfilePageViewModel : Conductor<object>, IHandle<MessageEvent>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _iProfile;
        private string _username;

        public UserProfilePageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profileEndpoint, IProfile iProfile)
        {
            _events = events;
            _user = user;
            _profileEndpoint = profileEndpoint;
            _iProfile = iProfile;

            _events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Profile = await _profileEndpoint.LoadProfileAsync(_username);

            NotifyOfPropertyChange(() => IsLogIn);
            NotifyOfPropertyChange(() => IsCurrentUser);
        }

        private Profile _profile;

        public Profile Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                NotifyOfPropertyChange(() => Profile);
            }
        }

        public bool IsLogIn => _user.Username == _username;

        public bool IsCurrentUser => _user.Username != _username;

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

            Profile = _iProfile as Profile;
        }

        public async Task UploadPhoto()
        {
            await ActivateItemAsync(IoC.Get<AddPhotoViewModel>(), new CancellationToken());
        }

        public async Task EditProfileAsync()
        {
            await ActivateItemAsync(IoC.Get<EditProfileViewModel>(), new CancellationToken());
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

            Profile = _iProfile as Profile;
        }
    }
}