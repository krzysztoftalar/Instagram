using Caliburn.Micro;
using DesktopUI.EventModels;
using System.Windows.Controls;

namespace DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for ChatPageView.xaml
    /// </summary>
    public partial class ChatPageView : UserControl, IHandle<CommentEvent>
    {
        public ChatPageView()
        {
            InitializeComponent();

            _events = IoC.Get<IEventAggregator>();

            _events.Subscribe(this);

            CommentsScrollViewer.ScrollToEnd();
        }

        private readonly IEventAggregator _events;

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange < 0)
            {
                if (CommentsScrollViewer.VerticalOffset == 0)
                {
                    _events.PublishOnUIThread(new MessageEvent { HandleGetNextComments = true });
                }
            }
        }

        public void Handle(CommentEvent message)
        {
            CommentsScrollViewer.ScrollToEnd();
        }
    }
}
