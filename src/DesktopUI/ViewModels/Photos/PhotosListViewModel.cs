using AutoMapper;
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;
using DesktopUI.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels.Photos
{
    public class PhotosListViewModel : BaseViewModel, IHandle<MessageEvent>, IHandle<ModeEvent>,
        IHandle<NavigationEvent>
    {
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private readonly IPhoto _photo;
        private readonly IMapper _mapper;
        private readonly PaginationHelper _pagination;

        private bool _isEditMode;
        private bool _isProfilePageActive;

        public PhotosListViewModel(IProfileEndpoint profileEndpoint, IEventAggregator events, IProfile profile,
            IAuthenticatedUser user, IPhoto photo, IMapper mapper)
        {
            _profileEndpoint = profileEndpoint;
            _events = events;
            _profile = profile;
            _user = user;
            _photo = photo;
            _mapper = mapper;

            _pagination = new PaginationHelper();

            events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            _pagination.Limit = 4;
            UserPhotos = new ObservableCollection<PhotoDisplayModel>();

            await LoadPhotosAsync(_pagination.Skip, _pagination.Limit);
        }

        public async Task LoadPhotosAsync(int skip, int limit)
        {
            string username = _isEditMode ? _profile.Username : _user.Username;

            var photosEnvelope = await _profileEndpoint.LoadPhotosAsync(username, skip, limit);
            var photos = _mapper.Map<List<PhotoDisplayModel>>(photosEnvelope.Photos);

            foreach (var photo in photos)
            {
                UserPhotos.Add(photo);
            }

            _pagination.ItemsCount = photosEnvelope.PhotosCount;
        }

        public async Task HandleGetNextAsync()
        {
            if (_pagination.PageNumber + 1 < _pagination.TotalPages)
            {
                _pagination.PageNumber++;

                await RunCommand(() => LoadingNext, async () =>
                {
                    await LoadPhotosAsync(_pagination.Skip, _pagination.Limit);
                });
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

        private ObservableCollection<PhotoDisplayModel> _userPhotos;

        public ObservableCollection<PhotoDisplayModel> UserPhotos
        {
            get => _userPhotos;
            set
            {
                _userPhotos = value;
                NotifyOfPropertyChange(() => UserPhotos);
            }
        }

        public bool IsLogIn => _user.Username == _profile.Username && _isEditMode;

        public bool IsPhotoSelected => SelectedPhoto != null;

        private PhotoDisplayModel _selectedPhoto;

        public PhotoDisplayModel SelectedPhoto
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
                        await _events.PublishOnUIThreadAsync(
                            new NavigationEvent { IsProfilePageActive = _isProfilePageActive }, new CancellationToken());
                        await _events.PublishOnUIThreadAsync(this, new CancellationToken());
                    });
                }
            }
        }

        public async Task SetMainPhotoAsync()
        {
            var photo = _mapper.Map<Photo>(SelectedPhoto);

            if (await _profileEndpoint.SetMainPhotoAsync(photo))
            {
                SelectedPhoto.IsMain = true;

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
            }
        }

        public async Task DeletePhotoAsync()
        {
            var photo = _mapper.Map<Photo>(SelectedPhoto);

            if (await _profileEndpoint.DeletePhotoAsync(photo))
            {
                UserPhotos.Remove(SelectedPhoto);

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
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