namespace DesktopUI.Library.Models
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }

        public void ResetUserModel()
        {
            DisplayName = "";
            Token = "";
            Username = "";
            Image = "";
        }
    }
}
