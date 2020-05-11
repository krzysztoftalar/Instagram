using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopUI.ViewModels
{
    public class EditProfileViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IEventAggregator _events;
        private readonly IProfileEndpoint _profileEndpoint;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;

        public EditProfileViewModel(IEventAggregator events, IProfileEndpoint profileEndpoint, IProfile profile,
            IAuthenticatedUser user)
        {
            _events = events;
            _profileEndpoint = profileEndpoint;
            _profile = profile;
            _user = user;

            _events.Subscribe(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            IsEditMode = true;
        }

        private string _displayName;

        public new string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        private string _bio;

        public string Bio
        {
            get => _bio;
            set
            {
                _bio = value;
                NotifyOfPropertyChange(() => Bio);
                NotifyOfPropertyChange(() => TextBoxBio);
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
                NotifyOfPropertyChange(() => TextBoxBio);
                NotifyOfPropertyChange(() => TextBoxDisplayName);
            }
        }

        private bool? _textBoxBio;

        public bool? TextBoxBio
        {
            get => _textBoxBio = IsEditMode == false || (!IsLogIn && !string.IsNullOrEmpty(_profile.Bio));
            set
            {
                _textBoxBio = value;
                NotifyOfPropertyChange(() => TextBoxBio);
            }
        }

        private bool? _textBoxDisplayName;

        public bool? TextBoxDisplayName
        {
            get => _textBoxDisplayName = IsEditMode == false;
            set
            {
                _textBoxDisplayName = value;
                NotifyOfPropertyChange(() => TextBoxDisplayName);
            }
        }

        public bool IsLogIn
        {
            get
            {
                bool output = _user.Username == _profile.Username;

                return output;
            }
        }

        public void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
        }

        public async Task Submit()
        {
            var profile = new ProfileFormValues
            {
                DisplayName = DisplayName,
                Bio = Bio
            };

            if (string.IsNullOrEmpty(DisplayName))
            {
                MessageBox.Show("Display name can not be empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            await _profileEndpoint.EditProfile(profile);

            MessageBox.Show("Profile edited successfully", "Congratulations!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

            _events.PublishOnUIThread(new MessageEvent());
        }

        public void Handle(MessageEvent message)
        {
            if (message.DisplayName != null)
            {
                DisplayName = message.DisplayName;
                Bio = message.Bio;
            }
        }
    }
}
