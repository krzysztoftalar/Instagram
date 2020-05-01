using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopUI.ViewModels
{
    public class AddPhotoViewModel : Screen
    {
        private readonly IProfileEndpoint _profile;
        private readonly IEventAggregator _events;

        public AddPhotoViewModel(IProfileEndpoint profile, IEventAggregator events)
        {
            _profile = profile;
            _events = events;
        }

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                NotifyOfPropertyChange(() => ImagePath);
            }
        }

        public void AddPhoto()
        {
            OpenFileDialog open = new OpenFileDialog
            {
                DefaultExt = (".png"),
                Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png"
            };

            if (open.ShowDialog() == true)
            {
                ImagePath = open.FileName;
            }
        }

        public async Task UploadPhoto()
        {
            try
            {
                await _profile.UploadPhoto(ImagePath);

                MessageBox.Show("Image uploaded successfully", "Congratulations!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                _events.PublishOnUIThread(new MessageEvent());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}