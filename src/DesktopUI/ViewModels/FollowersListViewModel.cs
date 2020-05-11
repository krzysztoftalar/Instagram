using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class FollowersListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IEventAggregator _events;
        
        private string _predicate;
        private int _limit = 10;
        private int _pageNumber = 1;
        private int _itemsCount;
        private int Skip => _limit * (_pageNumber - 1);

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

            await LoadFollowing(Skip, _limit);          
        }

        private async Task LoadFollowing(int skip, int limit)
        {
            var followers = await _profileEndpoint.LoadFollowing(_profile.Username, _predicate, skip, limit);
            UserFollowing = new BindableCollection<Profile>(followers.Followers);

            foreach (var profile in UserFollowing)
            {
                profile.Image = profile.Image ?? "../Assets/user.png";
            }

            _itemsCount = followers.FollowersCount;

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

            _events.PublishOnUIThread(new MessageEvent {Username = SelectedProfile.Username});
        }

        public async Task PrevPage()
        {
            _pageNumber--;

            await LoadFollowing(Skip, _limit);

            NotifyOfPropertyChange(() => IsPrevPage);
            NotifyOfPropertyChange(() => IsNextPage);
        }

        public async Task NextPage()
        {
            _pageNumber++;

            await LoadFollowing(Skip, _limit);

            NotifyOfPropertyChange(() => IsPrevPage);
            NotifyOfPropertyChange(() => IsNextPage);
        }

        private bool _isPrevPage;

        public bool IsPrevPage
        {
            get => _isPrevPage = _pageNumber > 1;
            set
            {
                _isPrevPage = value;
                NotifyOfPropertyChange(() => IsPrevPage);
            }
        }

        private bool _isNextPage;

        public bool IsNextPage
        {
            get => _isNextPage = _itemsCount > Skip + _limit;
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
                
                _pageNumber = 1;
                _limit = SelectedCount;

                LoadFollowing(Skip, _limit).ConfigureAwait(false);
            }
        }

        public void Handle(MessageEvent message)
        {
            _predicate = message.Predicate;
        }
    }
}