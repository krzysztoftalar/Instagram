using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class UserMainPageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private IAuthenticatedUser _user;
        private IProfile _profile;
        private readonly IUserEndpoint _userEndpoint;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser user, IProfile profile,
            IUserEndpoint userEndpoint)
        {
            _events = events;
            _user = user;
            _profile = profile;
            _userEndpoint = userEndpoint;

            ActivateItem(IoC.Get<PhotosListViewModel>());
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            _events.PublishOnUIThread(new MessageEvent { Username = _user.Username });
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

        private BindableCollection<AuthenticatedUser> _usersList;

        public BindableCollection<AuthenticatedUser> UsersList
        {
            get => _usersList;
            set
            {
                _usersList = value;
                NotifyOfPropertyChange(() => UsersList);
            }
        }

        private AuthenticatedUser _selectedUser;

        public AuthenticatedUser SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
                ViewProfile();
            }
        }

        public async Task SearchUsers()
        {
            var result = await _userEndpoint.SearchUsers(Search);
            UsersList = new BindableCollection<AuthenticatedUser>(result);

            foreach (var user in UsersList)
            {
                user.Image = user.Image ?? "../Assets/user.png";
            }
        }

        public void ViewProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);

            _events.PublishOnUIThread(new MessageEvent { Username = SelectedUser.Username });
        }

        public void EditProfile()
        {
            _events.PublishOnUIThread(Navigation.Profile);

            _events.PublishOnUIThread(new MessageEvent { Username = _user.Username });
        }

        public void Logout()
        {
            _profile = new Profile();
            _user = new AuthenticatedUser();

            _userEndpoint.LogOffUser();
            _events.PublishOnUIThread(Navigation.Login);
        }
    }
}