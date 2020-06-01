using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using Microsoft.Win32;
using System.Threading;
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
            var open = new OpenFileDialog
            {
                DefaultExt = (".png"),
                Filter = "Pictures (*.jpg;*.png;*.webp)|*.jpg;*.png;*.webp"
            };

            if (open.ShowDialog() == true)
            {
                ImagePath = open.FileName;
            }
        }

        public async Task UploadPhotoAsync()
        {
            if (await _profile.UploadPhotoAsync(ImagePath))
            {
                MessageBox.Show("Image uploaded successfully", "Congratulations!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                await _events.PublishOnUIThreadAsync(new MessageEvent(), new CancellationToken());

                ImagePath = "";
            }
            else
            {
                MessageBox.Show("Problem uploading the photo", "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}