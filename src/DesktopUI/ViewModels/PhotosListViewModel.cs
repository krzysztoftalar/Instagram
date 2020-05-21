using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class PhotosListViewModel : Screen, IHandle<MessageEvent>, IHandle<ModeEvent>, IHandle<NavigationEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private readonly IPhoto _photo;
        private readonly PaginationHelper _pagination;
        private bool _isEditMode;
        private bool _isProfilePageActive;

        public PhotosListViewModel(IProfileEndpoint profileEndpoint, IEventAggregator events, IProfile profile,
            IAuthenticatedUser user, IPhoto photo)
        {
            _profileEndpoint = profileEndpoint;
            _events = events;
            _profile = profile;
            _user = user;
            _photo = photo;

            _pagination = new PaginationHelper();

            events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            _pagination.Limit = 4;
            UserPhotos = new ObservableCollection<Photo>();

            await LoadPhotosAsync(_pagination.Skip, _pagination.Limit);
        }

        public async Task LoadPhotosAsync(int skip, int limit)
        {
            var username = _isEditMode ? _profile.Username : _user.Username;

            var photos = await _profileEndpoint.LoadPhotosAsync(username, skip, limit);

            foreach (var photo in photos.Photos)
            {
                UserPhotos.Add(photo);
            }

            _pagination.ItemsCount = photos.PhotosCount;
        }

        public async Task HandleGetNextAsync()
        {
            if (_pagination.PageNumber + 1 < _pagination.TotalPages)
            {
                _pagination.PageNumber++;

                LoadingNext = true;

                await LoadPhotosAsync(_pagination.Skip, _pagination.Limit);

                LoadingNext = false;
            }
        }

        private bool _loadingNext;

        public bool LoadingNext
        {
            get => _loadingNext;
            set
            {
                _loadingNext = value;
                NotifyOfPropertyChange(() => LoadingNext);
            }
        }

        private ObservableCollection<Photo> _userPhotos;

        public ObservableCollection<Photo> UserPhotos
        {
            get => _userPhotos;
            set
            {
                _userPhotos = value;
                NotifyOfPropertyChange(() => UserPhotos);
            }
        }

        private Photo _selectedPhoto;

        public Photo SelectedPhoto
        {
            get => _selectedPhoto;
            set
            {
                _selectedPhoto = value;
                NotifyOfPropertyChange(() => SelectedPhoto);
                NotifyOfPropertyChange(() => IsPhotoSelected);

                if (IsPhotoSelected)
                {
                    _photo.Id = SelectedPhoto.Id;
                    _photo.Url = SelectedPhoto.Url;
                }

                if (!IsLogIn)
                {
                    Task.Run(async () =>
                      {
                          await _events.PublishOnUIThreadAsync(Navigation.Chat, new CancellationToken());
                          await _events.PublishOnUIThreadAsync(new NavigationEvent
                          {
                              IsProfilePageActive = _isProfilePageActive
                          }, new CancellationToken());
                      });
                }
            }
        }

        public async Task SetMainPhotoAsync()
        {
            if (await _profileEndpoint.SetMainPhotoAsync(SelectedPhoto))
            {
                SelectedPhoto.IsMain = true;

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
            }
        }

        public async Task DeletePhotoAsync()
        {
            if (await _profileEndpoint.DeletePhotoAsync(SelectedPhoto))
            {
                UserPhotos.Remove(SelectedPhoto);

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
            }
        }

        public bool IsLogIn => _user.Username == _profile.Username && _isEditMode;

        private bool _isPhotoSelected;

        public bool IsPhotoSelected
        {
            get => _isPhotoSelected = SelectedPhoto != null;
            set
            {
                _isPhotoSelected = value;
                NotifyOfPropertyChange(() => IsPhotoSelected);
            }
        }

        public async Task HandleAsync(ModeEvent message, CancellationToken cancellationToken)
        {
            await Task.FromResult(_isEditMode = message.IsEditMode);
        }

        public async Task HandleAsync(MessageEvent message, CancellationToken cancellationToken)
        {
            if (message.HandleGetNextPhotos)
            {
                await HandleGetNextAsync();
            }
        }

        public async Task HandleAsync(NavigationEvent message, CancellationToken cancellationToken)
        {
            await Task.FromResult(_isProfilePageActive = message.IsProfilePageActive);
        }
    }
}