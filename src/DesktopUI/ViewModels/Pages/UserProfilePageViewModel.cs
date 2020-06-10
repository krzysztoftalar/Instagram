using AutoMapper;
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;
using DesktopUI.ViewModels.Photos;
using DesktopUI.ViewModels.Profiles;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels.Pages
{
    public class UserProfilePageViewModel : Conductor<object>, IHandle<MessageEvent>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _iProfile;
        private readonly IMapper _mapper;
        private string _username;

        public UserProfilePageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profileEndpoint, IProfile iProfile, IMapper mapper)
        {
            _events = events;
            _user = user;
            _profileEndpoint = profileEndpoint;
            _iProfile = iProfile;
            _mapper = mapper;

            _events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (_username != null)
            {
                var profile = await _profileEndpoint.LoadProfileAsync(_username);
                Profile = _mapper.Map<ProfileDisplayModel>(profile);
            }

            NotifyOfPropertyChange(() => IsLogIn);
            NotifyOfPropertyChange(() => IsCurrentUser);
        }

        private ProfileDisplayModel _profile;

        public ProfileDisplayModel Profile
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
            if (_iProfile.Following)
            {
                await _profileEndpoint.UnFollowAsync(_iProfile.Username);
            }
            else
            {
                await _profileEndpoint.FollowAsync(_iProfile.Username);
            }

            Profile = _iProfile as ProfileDisplayModel;
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

            await _events.PublishOnUIThreadAsync(new ModeEvent { IsEditMode = true, }, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new NavigationEvent { IsProfilePageActive = true },
                new CancellationToken());
        }

        public async Task LoadFollowersAsync(string predicate)
        {
            await ActivateItemAsync(IoC.Get<FollowersListViewModel>(), new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent { Predicate = predicate },
              new CancellationToken());
        }

        public async Task BackToMainPageAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Main, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new ModeEvent { IsEditMode = false }, new CancellationToken());

            await _events.PublishOnUIThreadAsync(this, new CancellationToken());
        }

        public async Task HandleAsync(MessageEvent message, CancellationToken cancellationToken)
        {
            await Task.FromResult(_username = message.Username);

            if (_username != null)
            {
                OnViewLoaded(this);

                await ActivateItemAsync(null, cancellationToken);
            }

            Profile = _iProfile as ProfileDisplayModel;
        }
    }
}