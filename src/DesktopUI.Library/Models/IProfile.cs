using System.Collections.Generic;

namespace DesktopUI.Library.Models
{
    public interface IProfile
    {
        string DisplayName { get; set; }
        string Image { get; set; }
        ICollection<Photo> Photos { get; set; }
        string Username { get; set; }
    }
}