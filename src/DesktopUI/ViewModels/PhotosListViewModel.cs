using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class PhotosListViewModel : Screen, IHandle<MessageEvent>, IHandle<ModeEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private readonly IPhoto _photo;
        private readonly IMessage _messageEvent;
        private readonly PaginationHelper _pagination;
        private bool _isEditMode;
        private bool _fromProfilePage;

        public PhotosListViewModel(IProfileEndpoint profileEndpoint, IEventAggregator events, IProfile profile,
            IAuthenticatedUser user, IPhoto photo, IMessage messageEvent)
        {
            _profileEndpoint = profileEndpoint;
            _events = events;
            _profile = profile;
            _user = user;
            _photo = photo;
            _messageEvent = messageEvent;

            _pagination = new PaginationHelper();

            _messageEvent.ProfilePage += OnProfilePage;

            events.Subscribe(this);
        }

        private void OnProfilePage(object sender, bool e)
        {
            _fromProfilePage = e;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            _pagination.Limit = 4;
            UserPhotos = new ObservableCollection<Photo>();

            await LoadPhotos(_pagination.Skip, _pagination.Limit);
        }

        public async Task LoadPhotos(int skip, int limit)
        {
            var username = _isEditMode ? _profile.Username : _user.Username;

            var photos = await _profileEndpoint.LoadPhotos(username, skip, limit);

            foreach (var photo in photos.Photos)
            {
                UserPhotos.Add(photo);
            }

            _pagination.ItemsCount = photos.PhotosCount;
        }

        public async Task HandleGetNext()
        {
            if (_pagination.PageNumber + 1 < _pagination.TotalPages)
            {
                _pagination.PageNumber++;

                LoadingNext = true;

                await LoadPhotos(_pagination.Skip, _pagination.Limit);

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
                    _events.PublishOnUIThread(Navigation.Chat);
                    _messageEvent.OnProfilePage(_fromProfilePage);
                }
            }
        }

        public async Task SetMainPhoto()
        {
            if (await _profileEndpoint.SetMainPhoto(SelectedPhoto))
            {
                SelectedPhoto.IsMain = true;

                await _events.PublishOnUIThreadAsync(new MessageEvent());
            }
        }

        public async Task DeletePhoto()
        {
            if (await _profileEndpoint.DeletePhoto(SelectedPhoto))
            {
                UserPhotos.Remove(SelectedPhoto);

                await _events.PublishOnUIThreadAsync(new MessageEvent());
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

        public void Handle(MessageEvent message)
        {
            if (message.HandleGetNextPhotos)
            {
                HandleGetNext().ConfigureAwait(false);
            }
        }

        public void Handle(ModeEvent message)
        {
            _isEditMode = message.IsEditMode;
        }
    }
}