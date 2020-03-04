namespace DesktopUI.Library.Models
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public AuthenticatedUser()
        {

        }

        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
}
