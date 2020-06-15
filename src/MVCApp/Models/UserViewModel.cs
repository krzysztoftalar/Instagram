using DesktopUI.Library.Models.DbModels;

namespace MVCApp.Models
{
    public class UserViewModel : IAuthenticatedUser
    {
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
    }
}
