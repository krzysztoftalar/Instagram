using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DesktopUI.Helpers;

namespace DesktopUI.ViewModels
{
    public class PhotosListViewModel : Screen, IHandle<MessageEvent>, IHandle<ModeEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private readonly IPhoto _photo;
        private readonly PaginationHelper _pagination;
        private bool _isEditMode;
        private bool _fromProfilePage;
        
        public PhotosListViewModel(IProfileEndpoint profileEndpoint, IEventAggregator events, IProfile profile,
            IAuthenticatedUser user, IPhoto photo)
        {
            _profileEndpoint = profileEndpoint;
            _events = events;
            _profile = profile;
            _user = user;
            _photo = photo;
           
            _pagination = new PaginationHelper();

            events.Subscribe(this);
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

                _loadingNext = true;
                NotifyOfPropertyChange(() => LoadingNext);

                await LoadPhotos(_pagination.Skip, _pagination.Limit);

                _loadingNext = false;
                NotifyOfPropertyChange(() => LoadingNext);
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
                NotifyOfPropertyChange(() => IsSelectedUser);

                if (SelectedPhoto != null)
                {
                    _photo.Id = SelectedPhoto.Id;
                    _photo.Url = SelectedPhoto.Url;
                }

                if (!IsLogIn)
                {
                    _events.PublishOnUIThread(Navigation.Chat);
                    _events.PublishOnUIThread(new MessageEvent { FromProfilePage = _fromProfilePage });
                }
            }
        }

        public async Task SetMainPhoto()
        {
            await _profileEndpoint.SetMainPhoto(SelectedPhoto);

            _events.PublishOnUIThread(new MessageEvent());
        }

        public async Task DeletePhoto()
        {
            await _profileEndpoint.DeletePhoto(SelectedPhoto);

            if (SelectedPhoto.IsMain)
            {
                _profile.Image = null;
                _user.Image = null;
            }

            UserPhotos.Remove(SelectedPhoto);

            _events.PublishOnUIThread(new MessageEvent());
        }

        public bool IsLogIn
        {
            get
            {
                var output = _user.Username == _profile.Username && _isEditMode;
                return output;
            }
        }

        private bool _isSelectedUser;

        public bool IsSelectedUser
        {
            get => _isSelectedUser = SelectedPhoto != null;
            set
            {
                _isSelectedUser = value;
                NotifyOfPropertyChange(() => IsSelectedUser);
            }
        }

        public void Handle(MessageEvent message)
        {
            _fromProfilePage = message.FromProfilePage;

            if (message.HandleGetNext)
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