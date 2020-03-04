namespace DesktopUI.Library.Models
{
    public interface IAuthenticatedUser
    {
        string DisplayName { get; set; }
        string Image { get; set; }
        string Token { get; set; }
        string Username { get; set; }
    }
}