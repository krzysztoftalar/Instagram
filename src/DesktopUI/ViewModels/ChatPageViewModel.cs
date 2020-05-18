using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Comment;
using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class ChatPageViewModel : Screen, IHandle<MessageEvent>
    {
        private readonly IChatHelper _chat;
        private readonly IEventAggregator _events;
        private readonly IProfile _profile;
        private readonly ICommentEndpoint _commentEndpoint;
        private readonly IAuthenticatedUser _user;
        private readonly IPhoto _photo;

        private bool _fromProfilePage;

        private int _limit = 10;
        private int _pageNumber;
        private int _itemsCount;
        private int Skip => _pageNumber * _limit;
        private int TotalPages => (int)Math.Ceiling((double)_itemsCount / _limit);

        public ChatPageViewModel(IChatHelper chat, IEventAggregator events, IProfile profile,
            ICommentEndpoint commentEndpoint, IAuthenticatedUser user, IPhoto photo)
        {
            _chat = chat;
            _events = events;
            _profile = profile;
            _commentEndpoint = commentEndpoint;
            _user = user;
            _photo = photo;

            _chat.GetReceive += OnGetReceive;

            _events.Subscribe(this);
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Comments = new ObservableCollection<Comment>();

            await _chat.CreateHubConnection(_photo.Id);

            await LoadComments(Skip, _limit);
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
        }

        public async Task LoadComments(int? skip, int limit)
        {
            var comments = await _commentEndpoint.LoadComments(_photo.Id, skip, limit);

            foreach (var comment in comments.Comments)
            {
                EvalComment(comment);

                Comments.Insert(0, comment);
            }

            _itemsCount = comments.CommentsCount;
        }

        public async Task HandleGetNext()
        {
            if (_pageNumber + 1 < TotalPages)
            {
                _pageNumber++;

                _loadingNext = true;
                NotifyOfPropertyChange(() => LoadingNext);

                await LoadComments(Skip, _limit);

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

        public async Task Send()
        {
            var comment = new Comment
            {
                PhotoId = _photo.Id,
                Body = Message
            };

            await _commentEndpoint.AddComment(comment);
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

        public async Task BackToMainPage()
        {
            await _chat.StopHubConnection(_photo.Id);

            _events.PublishOnUIThread(Navigation.Main);
        }

        public void Handle(MessageEvent message)
        {
            _fromProfilePage = message.FromProfilePage;

            NotifyOfPropertyChange(() => DisplayName);

            if (message.HandleGetNext)
            {
                HandleGetNext().ConfigureAwait(false);
            }
        }
    }
}
