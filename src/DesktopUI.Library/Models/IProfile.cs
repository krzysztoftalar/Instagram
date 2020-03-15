using System.Collections.Generic;

namespace DesktopUI.Library.Models
{
    public interface IProfile
    {
        string DisplayName { get; set; }
        string Image { get; set; }
        string Username { get; set; }
        bool Following { get; set; }
        int FollowingCount { get; set; }
        int FollowersCount { get; set; }
        ICollection<Photo> Photos { get; set; }

    }
}