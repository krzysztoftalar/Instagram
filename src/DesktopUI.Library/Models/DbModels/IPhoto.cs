namespace DesktopUI.Library.Models.DbModels
{
    public interface IPhoto
    {
        string Id { get; set; }
        string Url { get; set; }
        bool IsMain { get; set; }
    }
}
