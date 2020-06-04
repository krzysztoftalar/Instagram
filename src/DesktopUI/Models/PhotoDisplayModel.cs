using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Models
{
    public class PhotoDisplayModel : IPhoto
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}