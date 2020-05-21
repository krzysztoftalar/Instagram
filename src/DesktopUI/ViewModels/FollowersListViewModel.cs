using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class FollowersListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IEventAggregator _events;
        private readonly PaginationHelper _pagination;
        private string _predicate;

        public FollowersListViewModel(IProfileEndpoint profileEndpoint, IProfile profile, IEventAggregator events)
        {
            _profileEndpoint = profileEndpoint;
            _profile = profile;
            _events = events;

            _pagination = new PaginationHelper();

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadFollowingAsync(_pagination.Skip, _pagination.Limit);
        }

        private async Task LoadFollowingAsync(int skip, int limit)
        {
            var followers = await _profileEndpoint.LoadFollowingAsync(_profile.Username, _predicate, skip, limit);
            UserFollowing = new BindableCollection<Profile>(followers.Followers);

            foreach (var profile in UserFollowing)
            {
                profile.Image = profile.Image ?? "../Assets/user.png";
            }

            _pagination.ItemsCount = followers.FollowersCount;

            NotifyOfPropertyChange(() => IsPrevPage);
            NotifyOfPropertyChange(() => IsNextPage);
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

        private Profile _selectedProfile;

        public Profile SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                _selectedProfile = value;
                NotifyOfPropertyChange(() => SelectedProfile);

                ViewProfile();
            }
        }

        public void ViewProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);

            _events.PublishOnUIThread(new MessageEvent { Username = SelectedProfile.Username });
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

                LoadFollowingAsync(_pagination.Skip, _pagination.Limit).ConfigureAwait(false);
            }
        }

        public void Handle(MessageEvent message)
        {
            _predicate = message.Predicate;
        }
    }
}