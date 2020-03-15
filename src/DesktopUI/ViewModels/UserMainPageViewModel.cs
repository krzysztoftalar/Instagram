using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profiles;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DesktopUI.ViewModels
{
    public class UserMainPageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IUserEndpoint _userEndpoint;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profileEndpoint, IProfile profile, IUserEndpoint userEndpoint)
        {
            _events = events;
            _user = user;
            _profileEndpoint = profileEndpoint;
            _profile = profile;
            _userEndpoint = userEndpoint;

            ActivateItem(IoC.Get<PhotosListViewModel>());
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            _events.PublishOnUIThread(new MessageEvent { Message = _user.Username });
        }

        private string _image;

        public string Image
        {
            get => _image = _user.Image ?? "../Assets/user.png";
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        private string _displayName;

        public new string DisplayName
        {
            get => _displayName = _user.DisplayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        private string _searchUserImage;

        public string SearchUserImage
        {
            get => _searchUserImage ?? "../Assets/user.png";
            set
            {
                _searchUserImage = value;
                NotifyOfPropertyChange(() => SearchUserImage);
            }
        }

        private string _search;

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                NotifyOfPropertyChange(() => Search);
            }
        }

        private bool _isUserFind;

        public bool IsUserFind
        {
            get => _isUserFind;
            set
            {
                _isUserFind = value;
                NotifyOfPropertyChange(() => IsUserFind);
            }
        }

        private string _searchUserDisplayName;

        public string SearchUserDisplayName
        {
            get => _searchUserDisplayName;
            set
            {
                _searchUserDisplayName = value;
                NotifyOfPropertyChange(() => SearchUserDisplayName);
            }
        }

        public async Task SearchUsers()
        {
            IsUserFind = false;

            var result = await _profileEndpoint.LoadProfile(Search);

            if (!string.IsNullOrEmpty(result.Username))
            {
                IsUserFind = true;
                SearchUserImage = result.Image;
                SearchUserDisplayName = result.Username;
            }

            NotifyOfPropertyChange(() => FollowBtnContent);
            NotifyOfPropertyChange(() => FollowBtnBorder);
        }

        private string _followBtnContent;

        public string FollowBtnContent
        {
            get { return _profile.Following ? "UNFOLLOW" : "FOLLOW"; }
            set
            {
                _followBtnContent = value;
                NotifyOfPropertyChange(() => FollowBtnContent);
            }
        }

        private SolidColorBrush _followBtnBorder;

        public SolidColorBrush FollowBtnBorder
        {
            get { return _profile.Following ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green); }
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
                await _profileEndpoint.UnFollow(Search);
            }
            else
            {
                await _profileEndpoint.Follow(Search);
            }

            NotifyOfPropertyChange(() => FollowBtnContent);
            NotifyOfPropertyChange(() => FollowBtnBorder);
        }

        public void ViewProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);

            _events.PublishOnUIThread(new MessageEvent { Message = SearchUserDisplayName });
        }

        public void EditProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);

            _events.PublishOnUIThread(new MessageEvent { Message = _user.Username });
        }

        public void Logout()
        {
            _user.ResetUserModel();
            _userEndpoint.LogOffUser();
            _events.PublishOnUIThread(Navigation.Login);
        }
    }
}