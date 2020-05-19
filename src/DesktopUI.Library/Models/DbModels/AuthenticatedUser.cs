namespace DesktopUI.Library.Models.DbModels
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
}
