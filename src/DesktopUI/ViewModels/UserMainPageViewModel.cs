using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models.DbModels;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class UserMainPageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private readonly IUserEndpoint _userEndpoint;
        private readonly IAuthenticatedUser _iUser;
        private IProfile _profile;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser iUser, IProfile profile,
            IUserEndpoint userEndpoint)
        {
            _events = events;
            _iUser = iUser;
            _profile = profile;
            _userEndpoint = userEndpoint;

            ActivateItemAsync(IoC.Get<PhotosListViewModel>(), new CancellationToken());
        }

        public sealed override Task ActivateItemAsync(object item, CancellationToken cancellationToken)
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        private AuthenticatedUser _user;

        public AuthenticatedUser User
        {
            get => _user = _iUser as AuthenticatedUser;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
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

                Task.Run(ViewProfileAsync);
            }
        }

        public async Task SearchUsersAsync()
        {
            var users = await _userEndpoint.SearchUsersAsync(Search);
            UsersList = new BindableCollection<AuthenticatedUser>(users);
        }

        public async Task ViewProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Profile, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent
            {
                Username = SelectedUser.Username
            }, new CancellationToken());
        }

        public async Task EditProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Profile, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent
            {
                Username = _user.Username
            }, new CancellationToken());
        }

        public async Task LogoutAsync()
        {
            _profile = new Profile();
            _user = new AuthenticatedUser();

            _userEndpoint.LogOffUser();

            await _events.PublishOnUIThreadAsync(Navigation.Login, new CancellationToken());
        }
    }
}