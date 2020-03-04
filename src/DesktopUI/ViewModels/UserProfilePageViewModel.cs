using Caliburn.Micro;
using DesktopUI.Library.Api.Profile;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DesktopUI.ViewModels
{
    public class UserProfilePageViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IProfileEndpoint _profile;

        public UserProfilePageViewModel(IEventAggregator events, IProfileEndpoint profile)
        {
            _events = events;
            _profile = profile;
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

        public void UploadPhoto()
        {
            LoadImage();
        }

        private void LoadImage()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = (".png");
            open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

            if (open.ShowDialog() == true)
            {
                ImagePath = open.FileName;
            }
        }

        public async Task UploadImage()
        {
            try
            {
                await _profile.UpoloadPhoto(ImagePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
