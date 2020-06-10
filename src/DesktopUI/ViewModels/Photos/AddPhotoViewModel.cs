using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profile;
using DesktopUI.ViewModels.Base;
using Microsoft.Win32;

namespace DesktopUI.ViewModels.Photos
{
    public class AddPhotoViewModel : BaseViewModel
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

        private bool _uploadIsRunning;

        public bool UploadIsRunning
        {
            get => _uploadIsRunning;
            set
            {
                _uploadIsRunning = value;
                NotifyOfPropertyChange(() => UploadIsRunning);
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
            await RunCommand(() => UploadIsRunning, async () =>
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
            });
        }
    }
}