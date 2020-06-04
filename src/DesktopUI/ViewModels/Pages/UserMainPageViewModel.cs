using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.User;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;
using DesktopUI.ViewModels.Photos;

namespace DesktopUI.ViewModels.Pages
{
    public class UserMainPageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private readonly IUserEndpoint _userEndpoint;
        private readonly IMapper _mapper;
        private IAuthenticatedUser _iUser;
        private IProfile _profile;

        public UserMainPageViewModel(IEventAggregator events, IAuthenticatedUser iUser, IProfile profile,
            IUserEndpoint userEndpoint, IMapper mapper)
        {
            _events = events;
            _iUser = iUser;
            _profile = profile;
            _userEndpoint = userEndpoint;
            _mapper = mapper;

            ActivateItemAsync(IoC.Get<PhotosListViewModel>(), new CancellationToken());
        }

        public sealed override Task ActivateItemAsync(object item, CancellationToken cancellationToken)
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        private UserDisplayModel _user;

        public UserDisplayModel User
        {
            get => _user = _iUser as UserDisplayModel;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
            }
        }

        private BindableCollection<UserDisplayModel> _usersList;

        public BindableCollection<UserDisplayModel> UsersList
        {
            get => _usersList;
            set
            {
                _usersList = value;
                NotifyOfPropertyChange(() => UsersList);
            }
        }

        private UserDisplayModel _selectedUser;

        public UserDisplayModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);

                Task.Run(ViewProfileAsync);
            }
        }

        public async Task SearchUsersAsync(string search)
        {
            var usersList = await _userEndpoint.SearchUsersAsync(search);
            var users = _mapper.Map<List<UserDisplayModel>>(usersList);

            UsersList = new BindableCollection<UserDisplayModel>(users);
        }

        public async Task ViewProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Profile, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent {Username = SelectedUser.Username},
                new CancellationToken());
        }

        public async Task EditProfileAsync()
        {
            await _events.PublishOnUIThreadAsync(Navigation.Profile, new CancellationToken());

            await _events.PublishOnUIThreadAsync(new MessageEvent {Username = _user.Username}, new CancellationToken());
        }

        public async Task LogoutAsync()
        {
            _profile = new ProfileDisplayModel();
            _iUser = new UserDisplayModel();

            _userEndpoint.LogOffUser();

            await _events.PublishOnUIThreadAsync(Navigation.Login, new CancellationToken());
        }
    }
}