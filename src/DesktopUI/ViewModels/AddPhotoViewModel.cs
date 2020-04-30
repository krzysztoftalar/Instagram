using Caliburn.Micro;
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

        public AddPhotoViewModel(IProfileEndpoint profile)
        {
            _profile = profile;
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}