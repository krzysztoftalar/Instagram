using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using DesktopUI.EventModels;

namespace DesktopUI.Views.Pages
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
            _events.SubscribeOnPublishedThread(this);

            CommentsScrollViewer.ScrollToEnd();
        }

        private readonly IEventAggregator _events;

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange < 0)
            {
                if (CommentsScrollViewer.VerticalOffset == 0)
                {
                    Task.Run(() => _events.PublishOnUIThreadAsync(new MessageEvent {HandleGetNextComments = true}, new CancellationToken()));
                }
            }
        }

        public async Task HandleAsync(CommentEvent message, CancellationToken cancellationToken)
        {
            if (message.ScrollToEnd)
            {
                CommentsScrollViewer.ScrollToEnd();
            }

            if (message.ScrollToVerticalOffset)
            {
                CommentsScrollViewer.ScrollToVerticalOffset(500);
            }

            await Task.CompletedTask;
        }
    }
}
