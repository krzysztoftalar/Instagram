using System.Collections.Generic;

namespace DesktopUI.Library.Models
{
    public class Profile : IProfile
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
