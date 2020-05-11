using Caliburn.Micro;
using DesktopUI.EventModels;
using System.Windows.Controls;

namespace DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for PhotosListView.xaml
    /// </summary>
    public partial class PhotosListView : UserControl
    {
        public PhotosListView()
        {
            InitializeComponent();

            _events = IoC.Get<IEventAggregator>();
        }

        private readonly IEventAggregator _events;

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange > 0)
            {
                if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight)
                {
                    _events.PublishOnUIThread(new MessageEvent { HandleGetNext = true });
                }
            }
        }
    }
}
