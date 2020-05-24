using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopUI.ViewModels
{
    public class EditProfileViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _iProfile;
        private readonly IAuthenticatedUser _user;

        public EditProfileViewModel(IEventAggregator events, IProfileEndpoint profileEndpoint, IProfile iProfile,
            IAuthenticatedUser user)
        {
            _events = events;
            _profileEndpoint = profileEndpoint;
            _iProfile = iProfile;
            _user = user;

            _events.SubscribeOnPublishedThread(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            IsEditMode = true;
        }

        private Profile _profile;

        public Profile Profile
        {
            get => _profile = _iProfile as Profile;
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

        public async Task SubmitAsync()
        {
            var profile = new ProfileFormValues
            {
                DisplayName = Profile.DisplayName,
                Bio = Profile.Bio
            };

            if (string.IsNullOrEmpty(DisplayName))
            {
                MessageBox.Show("Display name can not be empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (await _profileEndpoint.EditProfileAsync(profile))
            {
                MessageBox.Show("Profile edited successfully", "Congratulations!",
                  MessageBoxButton.OK, MessageBoxImage.Information);

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
            }
        }
    }
}
