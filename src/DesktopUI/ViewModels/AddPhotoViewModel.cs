using Caliburn.Micro;
using Microsoft.Win32;
using System.Windows.Controls;

namespace DesktopUI.ViewModels
{
    public class AddPhotoViewModel : Screen
    {
        public AddPhotoViewModel()
        {
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

        public void UploadImage()
        {
            LoadImage();
        }

        private void LoadImage()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = (".png");
            open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

            if (open.ShowDialog() == true)
                ImagePath = open.FileName;
        }

        //private BitmapImage LoadImageFromFile(string filename)
        //{
        //    using (var fs = File.OpenRead(filename))
        //    {

        //        var img = new BitmapImage();
        //        img.BeginInit();
        //        img.UriSource = new Uri(filename, UriKind.Absolute);
        //        img.CacheOption = BitmapCacheOption.OnLoad;
        //        img.DecodePixelWidth = (int)SystemParameters.PrimaryScreenWidth;
        //        img.StreamSource = fs;
        //        img.EndInit();
        //        return img;
        //    }
        //}

        //public void FileDropped(object sender, DragEventArgs e)
        //{
        //    var data = e.Data as DataObject;
        //    if (data.ContainsFileDropList())
        //    {
        //        var files = data.GetFileDropList();
        //        //Viewer.Source = LoadImageFromFile(files[0]);
        //    }
        //}
    }
}
