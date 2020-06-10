using Caliburn.Micro;
using DesktopUI.Commands;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;
using DesktopUI.ViewModels.Base;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoMapper;

namespace DesktopUI.ViewModels.Profiles
{
    public class EditProfileViewModel : BaseViewModel
    {
        private readonly IEventAggregator _events;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _iProfile;
        private readonly IAuthenticatedUser _user;
        private readonly IMapper _mapper;

        public EditProfileViewModel(IEventAggregator events, IProfileEndpoint profileEndpoint, IProfile iProfile,
            IAuthenticatedUser user, IMapper mapper)
        {
            _events = events;
            _profileEndpoint = profileEndpoint;
            _iProfile = iProfile;
            _user = user;
            _mapper = mapper;

            _events.SubscribeOnPublishedThread(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            IsEditMode = true;
        }

        private ICommand _submitCommand;

        public ICommand SubmitCommand =>
            _submitCommand ??= new RelayParameterizedCommand<ProfileDisplayModel>(
                async (profile) => await SubmitAsync(profile),
                profile => !string.IsNullOrWhiteSpace(profile?.DisplayName));

        private ProfileDisplayModel _profile;

        public ProfileDisplayModel Profile
        {
            get => _profile = _iProfile as ProfileDisplayModel;
            set
            {
                _profile = value;
                NotifyOfPropertyChange(() => Profile);
            }
        }

        private bool _isEditMode;

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                NotifyOfPropertyChange(() => IsEditMode);
                NotifyOfPropertyChange(() => IsBioVisible);
                NotifyOfPropertyChange(() => IsDisplayNameVisible);
            }
        }

        private bool _isBioVisible;

        public bool IsBioVisible
        {
            get => _isBioVisible = (IsEditMode == false) || (!IsLogIn && !string.IsNullOrEmpty(_iProfile.Bio));
            set
            {
                _isBioVisible = value;
                NotifyOfPropertyChange(() => IsBioVisible);
            }
        }

        private bool _isDisplayNameVisible;

        public bool IsDisplayNameVisible
        {
            get => _isDisplayNameVisible = IsEditMode == false;
            set
            {
                _isDisplayNameVisible = value;
                NotifyOfPropertyChange(() => IsDisplayNameVisible);
            }
        }

        public bool IsLogIn => _user.Username == _iProfile.Username;

        public void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
        }

        public async Task SubmitAsync(ProfileDisplayModel profile)
        {
            if (await _profileEndpoint.EditProfileAsync(_mapper.Map<ProfileFormValues>(profile)))
            {
                MessageBox.Show("Profile edited successfully", "Congratulations!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
            }
        }
    }
}