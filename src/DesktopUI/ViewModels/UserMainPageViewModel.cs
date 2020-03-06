using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profiles;
using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class UserMainPageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IProfileEndpoint _profile;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profile)
        {
            _events = events;
            _user = user;
            _profile = profile;

            ActivateItem(IoC.Get<PhotosListViewModel>());
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Image = _user.Image;
            DisplayName = _user.DisplayName;        
                      
            _events.PublishOnUIThread(new MessageEvent { Message = _user.DisplayName });
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

        private string _displayName;

        public new string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        private string _searchUserImage;

        public string SearchUserImage
        {
            get => _searchUserImage;
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

            var result = await _profile.LoadProfile(Search);

            if (result.Username.Length > 0)
            {
                IsUserFind = true;
                Search = "";
                SearchUserImage = result.Image;
                SearchUserDisplayName = result.DisplayName;
            }
        }

        public void ViewProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);

            _events.PublishOnUIThread(new MessageEvent { Message = SearchUserDisplayName });
        }

        public void EditProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);
        }
    }
}