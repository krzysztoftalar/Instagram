using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class PhotosListViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private bool _isEditMode;      

        private int _limit = 4;
        private int _pageNumber;
        private int _itemsCount;
        private int Skip => _pageNumber * _limit;
        private int TotalPages => (int)Math.Ceiling((double)_itemsCount / _limit);

        public PhotosListViewModel(IProfileEndpoint profileEndpoint, IEventAggregator events, IProfile profile,
            IAuthenticatedUser user)
        {
            _profileEndpoint = profileEndpoint;
            _events = events;
            _profile = profile;
            _user = user;

            events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            UserPhotos = new ObservableCollection<Photo>();

            await LoadPhotos(Skip, _limit);
        }

        public async Task LoadPhotos(int skip, int limit)
        {
            var username = _isEditMode ? _profile.Username : _user.Username;

            var photos = await _profileEndpoint.LoadPhotos(username, skip, limit);

            foreach (var photo in photos.Photos)
            {
                UserPhotos.Add(photo);
            }

            _itemsCount = photos.PhotosCount;
        }

        public async Task HandleGetNext()
        {
            if (_pageNumber + 1 < TotalPages)
            {
                _pageNumber++;

                _loadingNext = true;
                NotifyOfPropertyChange(() => LoadingNext);

                await LoadPhotos(Skip, _limit);

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
            _isEditMode = message.IsEditMode;

            if (message.HandleGetNext)
            {
                HandleGetNext().ConfigureAwait(false);
            }
        }
    }
}