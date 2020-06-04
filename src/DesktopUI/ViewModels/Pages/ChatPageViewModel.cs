using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Helpers;
using DesktopUI.Library.Api.Comment;
using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models.DbModels;
using DesktopUI.Models;

namespace DesktopUI.ViewModels.Pages
{
    public class ChatPageViewModel : Screen, IHandle<MessageEvent>, IHandle<NavigationEvent>
    {
        private readonly IChatHelper _chat;
        private readonly ICommentEndpoint _commentEndpoint;
        private readonly IEventAggregator _events;
        private readonly IPhoto _photo;
        private readonly IMapper _mapper;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;
        private readonly PaginationHelper _pagination;
        private bool _isProfilePageActive;

        public ChatPageViewModel(IChatHelper chat, IEventAggregator events, IProfile profile,
            ICommentEndpoint commentEndpoint, IAuthenticatedUser user, IPhoto photo, IMapper mapper)
        {
            _chat = chat;
            _events = events;
            _profile = profile;
            _commentEndpoint = commentEndpoint;
            _user = user;
            _photo = photo;
            _mapper = mapper;

            _pagination = new PaginationHelper();

            _chat.GetReceive += OnGetReceive;

            _events.SubscribeOnPublishedThread(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Comments = new ObservableCollection<CommentDisplayModel>();

            await _chat.CreateHubConnectionAsync(_photo.Id);

            await LoadCommentsAsync(_pagination.Skip, _pagination.Limit);
        }

        private void OnGetReceive(object sender, Comment comment)
        {
            var commentModel = _mapper.Map<CommentDisplayModel>(comment);

            EvalComment(commentModel);

            Comments.Add(commentModel);

            _events.PublishOnUIThreadAsync(new CommentEvent {ScrollToEnd = true}, new CancellationToken());
        }

        public void EvalComment(CommentDisplayModel comment)
        {
            comment.IsLoggedInComment = comment.Username == _user.Username;
            comment.Date = comment.CreatedAt.ToString("dd-MM-yyyy");
        }

        public async Task LoadCommentsAsync(int? skip, int limit)
        {
            var commentsEnvelope = await _commentEndpoint.LoadCommentsAsync(_photo.Id, skip, limit);
            var comments = _mapper.Map<List<CommentDisplayModel>>(commentsEnvelope.Comments);

            foreach (var comment in comments)
            {
                EvalComment(comment);

                Comments.Insert(0, comment);
            }

            _pagination.ItemsCount = commentsEnvelope.CommentsCount;
        }


        public async Task HandleGetNextAsync()
        {
            if (_pagination.PageNumber + 1 < _pagination.TotalPages)
            {
                _pagination.PageNumber++;

                LoadingNext = true;

                await _events.PublishOnUIThreadAsync(new CommentEvent {ScrollToVerticalOffset = true},
                    new CancellationToken());

                await LoadCommentsAsync(_pagination.Skip, _pagination.Limit);

                LoadingNext = false;
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

        private ObservableCollection<CommentDisplayModel> _comments;

        public ObservableCollection<CommentDisplayModel> Comments
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
            get => _isProfilePageActive ? _profile.DisplayName : _user.DisplayName;
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

        public async Task SendMessageAsync()
        {
            var comment = new Comment
            {
                PhotoId = _photo.Id,
                Body = Message
            };

            if (!string.IsNullOrWhiteSpace(Message))
            {
                await _commentEndpoint.AddCommentAsync(comment);

                Message = "";
            }
        }

        private CommentDisplayModel _selectedComment;

        public CommentDisplayModel SelectedComment
        {
            get => _selectedComment;
            set
            {
                _selectedComment = value;
                NotifyOfPropertyChange(() => SelectedComment);
                NotifyOfPropertyChange(() => IsCommentSelected);
            }
        }

        private bool _isCommentSelected;

        public bool IsCommentSelected
        {
            get => _isCommentSelected = SelectedComment != null && SelectedComment.IsLoggedInComment;
            set
            {
                _isCommentSelected = value;
                NotifyOfPropertyChange(() => IsCommentSelected);
            }
        }

        public async Task DeleteCommentAsync()
        {
            var comment = _mapper.Map<Comment>(SelectedComment);

            if (await _commentEndpoint.DeleteCommentAsync(comment))
            {
                Comments.Remove(SelectedComment);
            }
        }

        public async Task BackToMainPageAsync()
        {
            await _chat.StopHubConnectionAsync(_photo.Id);

            await _events.PublishOnUIThreadAsync(Navigation.Main, new CancellationToken());
        }

        public async Task HandleAsync(MessageEvent message, CancellationToken cancellationToken)
        {
            if (message.HandleGetNextComments)
            {
                await HandleGetNextAsync();
            }
        }

        public async Task HandleAsync(NavigationEvent message, CancellationToken cancellationToken)
        {
            await Task.FromResult(_isProfilePageActive = message.IsProfilePageActive);

            NotifyOfPropertyChange(() => DisplayName);
        }
    }
}