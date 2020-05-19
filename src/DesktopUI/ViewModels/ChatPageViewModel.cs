using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Comment;
using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class ChatPageViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IChatHelper _chat;
        private readonly ICommentEndpoint _commentEndpoint;
        private readonly IEventAggregator _events;
        private readonly IPhoto _photo;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private readonly PaginationHelper _pagination;
        private bool _fromProfilePage;

        public ChatPageViewModel(IChatHelper chat, IEventAggregator events, IProfile profile,
            ICommentEndpoint commentEndpoint, IAuthenticatedUser user, IPhoto photo)
        {
            _chat = chat;
            _events = events;
            _profile = profile;
            _commentEndpoint = commentEndpoint;
            _user = user;
            _photo = photo;

            _pagination = new PaginationHelper();

            _chat.GetReceive += OnGetReceive;

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Comments = new ObservableCollection<Comment>();

            await _chat.CreateHubConnection(_photo.Id);

            await LoadComments(_pagination.Skip, _pagination.Limit);
        }

        private void OnGetReceive(object sender, Comment comment)
        {
            EvalComment(comment);

            Comments.Add(comment);

            _events.PublishOnUIThread(new CommentEvent());
        }

        public void EvalComment(Comment comment)
        {
            comment.IsLoggedInComment = comment.Username == _user.Username;
            comment.Image = comment.Image ?? "../Assets/user.png";
            comment.Date = comment.CreatedAt.ToString("dd-MM-yyyy");
        }

        public async Task LoadComments(int? skip, int limit)
        {
            var comments = await _commentEndpoint.LoadComments(_photo.Id, skip, limit);

            foreach (var comment in comments.Comments)
            {
                EvalComment(comment);

                Comments.Insert(0, comment);
            }

            _pagination.ItemsCount = comments.CommentsCount;
        }


        public async Task HandleGetNext()
        {
            if (_pagination.PageNumber + 1 < _pagination.TotalPages)
            {
                _pagination.PageNumber++;

                _loadingNext = true;
                NotifyOfPropertyChange(() => LoadingNext);

                await LoadComments(_pagination.Skip, _pagination.Limit);

                _loadingNext = false;
                NotifyOfPropertyChange(() => LoadingNext);
            }
        }

        private bool _loadingNext;

        public bool LoadingNext
        {
            get => _loadingNext;
            set
            {
                _loadingNext = value;
                NotifyOfPropertyChange(() => LoadingNext);
            }
        }

        private ObservableCollection<Comment> _comments;

        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                NotifyOfPropertyChange(() => Comments);
            }
        }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        private string _displayName;

        public new string DisplayName
        {
            get => _fromProfilePage ? _profile.DisplayName : _user.DisplayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        private string _image;

        public string Image
        {
            get => _image = _photo.Url;
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        public async Task Send()
        {
            var comment = new Comment
            {
                PhotoId = _photo.Id,
                Body = Message
            };

            if (!string.IsNullOrWhiteSpace(Message))
            {
                await _commentEndpoint.AddComment(comment);
            }
        }

        public async Task BackToMainPage()
        {
            await _chat.StopHubConnection(_photo.Id);

            _events.PublishOnUIThread(Navigation.Main);
        }

        public void Handle(MessageEvent message)
        {
            _fromProfilePage = message.FromProfilePage;

            NotifyOfPropertyChange(() => DisplayName);

            if (message.HandleGetNextComments)
            {
                HandleGetNext().ConfigureAwait(false);
            }
        }
    }
}