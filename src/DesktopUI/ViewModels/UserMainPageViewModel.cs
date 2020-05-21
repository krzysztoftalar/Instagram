using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models.DbModels;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class UserMainPageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private readonly IUserEndpoint _userEndpoint;
        private IAuthenticatedUser _user;
        private IProfile _profile;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser user, IProfile profile,
            IUserEndpoint userEndpoint)
        {
            _events = events;
            _user = user;
            _profile = profile;
            _userEndpoint = userEndpoint;

            ActivateItem(IoC.Get<PhotosListViewModel>());
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

                ViewProfileAsync().ConfigureAwait(false);
            }
        }

        public async Task SearchUsersAsync()
        {
            var users = await _userEndpoint.SearchUsersAsync(Search);
            UsersList = new BindableCollection<AuthenticatedUser>(users);

            foreach (var user in UsersList)
            {
                user.Image = user.Image ?? "../Assets/user.png";
            }
        }

        public async Task ViewProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Profile);

            await _events.PublishOnUIThreadAsync(new MessageEvent { Username = SelectedUser.Username });
        }

        public async Task EditProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Profile);

            await _events.PublishOnUIThreadAsync(new MessageEvent { Username = _user.Username });
        }

        public async Task LogoutAsync()
        {
            _profile = new Profile();
            _user = new AuthenticatedUser();

            _userEndpoint.LogOffUser();
            await _events.PublishOnUIThreadAsync(Navigation.Login);
        }
    }
}