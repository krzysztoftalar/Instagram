﻿using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System.Threading;
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

            _events.SubscribeOnPublishedThread(this);
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
            get => _textBoxBio = (IsEditMode == false) || (!IsLogIn && !string.IsNullOrEmpty(_profile.Bio));
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

        public bool IsLogIn => _user.Username == _profile.Username;

        public void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
        }

        public async Task SubmitAsync()
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

            if (await _profileEndpoint.EditProfileAsync(profile))
            {
                MessageBox.Show("Profile edited successfully", "Congratulations!",
                  MessageBoxButton.OK, MessageBoxImage.Information);

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());
            }
        }

        public async Task HandleAsync(MessageEvent message, CancellationToken cancellationToken)
        {
            if (message.DisplayName != null)
            {
                await Task.FromResult(DisplayName = message.DisplayName);
                await Task.FromResult(Bio = message.Bio);
            }
        }
    }
}
