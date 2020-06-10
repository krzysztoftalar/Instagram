using AutoMapper;
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels.Profiles
{
    public class FollowersListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IEventAggregator _events;
        private readonly IMapper _mapper;
        private readonly PaginationHelper _pagination;
        private string _predicate;

        public FollowersListViewModel(IProfileEndpoint profileEndpoint, IProfile profile, IEventAggregator events,
            IMapper mapper)
        {
            _profileEndpoint = profileEndpoint;
            _profile = profile;
            _events = events;
            _mapper = mapper;

            _pagination = new PaginationHelper();

            _events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadFollowingAsync(_pagination.Skip, _pagination.Limit);
        }

        private async Task LoadFollowingAsync(int skip, int limit)
        {
            var followersEnvelope = await _profileEndpoint.LoadFollowingAsync(_profile.Username, _predicate, skip, limit);
            var followers = _mapper.Map<List<ProfileDisplayModel>>(followersEnvelope.Followers);

            UserFollowing = new BindableCollection<ProfileDisplayModel>(followers);

            _pagination.ItemsCount = followersEnvelope.FollowersCount;

            NotifyOfPropertyChange(() => IsPrevPage);
            NotifyOfPropertyChange(() => IsNextPage);
        }

        private BindableCollection<ProfileDisplayModel> _userFollowing;

        public BindableCollection<ProfileDisplayModel> UserFollowing
        {
            get => _userFollowing;
            set
            {
                _userFollowing = value;
                NotifyOfPropertyChange(() => UserFollowing);
            }
        }

        private ProfileDisplayModel _selectedProfile;

        public ProfileDisplayModel SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                _selectedProfile = value;
                NotifyOfPropertyChange(() => SelectedProfile);

                Task.Run(ViewProfileAsync);
            }
        }

        public async Task ViewProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(new MessageEvent {Username = SelectedProfile.Username},
                new CancellationToken());
        }

        public async Task PrevPageAsync()
        {
            _pagination.PageNumber--;

            await LoadFollowingAsync(_pagination.Skip, _pagination.Limit);

            NotifyOfPropertyChange(() => IsPrevPage);
            NotifyOfPropertyChange(() => IsNextPage);
        }

        public async Task NextPageAsync()
        {
            _pagination.PageNumber++;

            await LoadFollowingAsync(_pagination.Skip, _pagination.Limit);

            NotifyOfPropertyChange(() => IsPrevPage);
            NotifyOfPropertyChange(() => IsNextPage);
        }

        private bool _isPrevPage;

        public bool IsPrevPage
        {
            get => _isPrevPage = _pagination.PageNumber > 0;
            set
            {
                _isPrevPage = value;
                NotifyOfPropertyChange(() => IsPrevPage);
            }
        }

        private bool _isNextPage;

        public bool IsNextPage
        {
            get => _isNextPage = _pagination.PageNumber + 1 < _pagination.TotalPages;
            set
            {
                _isNextPage = value;
                NotifyOfPropertyChange(() => IsNextPage);
            }
        }

        private int _selectedCount;

        public int SelectedCount
        {
            get => _selectedCount;
            set
            {
                _selectedCount = value;
                NotifyOfPropertyChange(() => SelectedCount);

                _pagination.PageNumber = 0;
                _pagination.Limit = SelectedCount;

                Task.Run(() => LoadFollowingAsync(_pagination.Skip, _pagination.Limit));
            }
        }

        public async Task HandleAsync(MessageEvent message, CancellationToken cancellationToken)
        {
            await Task.FromResult(_predicate = message.Predicate);
        }
    }
}