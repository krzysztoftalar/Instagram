namespace DesktopUI.Library.Models
{
    public interface IPhoto
    {
        string Id { get; set; }
        string Url { get; set; }
        bool IsMain { get; set; }
    }
}
