using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Api.Profiles;
using DesktopUI.Library.Models;

namespace DesktopUI.ViewModels
{
    public class UserProfilePageViewModel : Conductor<object>, IHandle<MessageEvent>
    {
        private readonly IEventAggregator _events;
        private readonly IAuthenticatedUser _user;
        private readonly IProfileEndpoint _profileEndpoint;
        private string _username;

        public UserProfilePageViewModel(IEventAggregator events, IAuthenticatedUser user,
            IProfileEndpoint profileEndpoint)
        {
            _events = events;
            _user = user;
            _profileEndpoint = profileEndpoint;

            _events.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var result = await _profileEndpoint.LoadProfile(_username);
            FollowersCount = result.FollowersCount.ToString();
            FollowingCount = result.FollowingCount.ToString();
        }

        public bool IsCurrentUser
        {
            get
            {
                bool output = _user.Username == _username;

                return output;
            }
        }

        private string _followersCount;

        public string FollowersCount
        {
            get => _followersCount;
            set
            {
                _followersCount = value;
                NotifyOfPropertyChange(() => FollowersCount);
            }
        }

        private string _followingCount;

        public string FollowingCount
        {
            get => _followingCount;
            set
            {
                _followingCount = value;
                NotifyOfPropertyChange(() => FollowingCount);
            }
        }

        public void UploadPhoto()
        {
            ActivateItem(IoC.Get<AddPhotoViewModel>());
        }

        public void PhotosList()
        {
            ActivateItem(IoC.Get<PhotosListViewModel>());

            _events.PublishOnUIThread(new MessageEvent { Message = _username });
        }

        public void LoadFollowing()
        {
            ActivateItem(IoC.Get<FollowersListViewModel>());

            _events.PublishOnUIThread(new MessageEvent { Message = "following" });
        }

        public void LoadFollowers()
        {
            ActivateItem(IoC.Get<FollowersListViewModel>());

            _events.PublishOnUIThread(new MessageEvent { Message = "followers" });
        }

        public void BackToMainPage()
        {
            _events.PublishOnUIThread(Navigation.Main);
        }

        public void Handle(MessageEvent message)
        {
            _username = message.Message;
        }
    }
}
