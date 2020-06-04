using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Models
{
    public class UserDisplayModel : IAuthenticatedUser
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
}