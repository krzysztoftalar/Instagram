using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Models;
using System.Windows.Controls;

namespace DesktopUI.ViewModels
{
    public class UserProfilePageViewModel : Conductor<object>
    {
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;

        public UserProfilePageViewModel(IEventAggregator events, IProfile profile, IAuthenticatedUser user)
        {
            _events = events;
            _profile = profile;
            _user = user;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            //IsLoggedIn = true;
        }

        private Image _viewer;

        public Image Viewer
        {
            get { return _viewer; }
            set
            {
                _viewer = value;
                NotifyOfPropertyChange(() => Viewer);
            }
        }

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                NotifyOfPropertyChange(() => ImagePath);
            }
        }

        public bool IsCurrentUser
        {
            get
            {
                bool output = _user.DisplayName == _profile.DisplayName;

                return output;
            }
        }

        public void UploadPhoto()
        {
            ActivateItem(IoC.Get<AddPhotoViewModel>());
        }

        public void PhotosList()
        {
            ActivateItem(IoC.Get<PhotosListViewModel>());

            _events.PublishOnUIThread(new MessageEvent { Message = _profile.DisplayName });
        }

        public void BackToMainPage()
        {
            _events.PublishOnUIThread(Navigation.Main);
        }
    }
}
