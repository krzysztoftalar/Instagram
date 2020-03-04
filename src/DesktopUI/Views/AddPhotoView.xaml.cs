using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for AddPhotoView.xaml
    /// </summary>
    public partial class AddPhotoView : UserControl
    {
        public AddPhotoView()
        {
            InitializeComponent();
            // this.DataContext = this;           

        }

  

        //private void OnDrop(object sender, DragEventArgs e)
        //{
        //    var data = e.Data as DataObject;
        //    if (data.ContainsFileDropList())
        //    {
        //        var files = data.GetFileDropList();
        //        Viewer.Source = LoadImageFromFile(files[0]);
        //    }
        //}

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

        //private Image _imageViewer;
        //public Image ImageViewer
        //{
        //    get { return _imageViewer; }
        //    set
        //    {
        //        _imageViewer = value;
        //        OnPropertyChanged("ImageViewer");
        //    }
        //}


        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;

        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }
}
