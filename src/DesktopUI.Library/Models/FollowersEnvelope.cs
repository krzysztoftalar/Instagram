using System.Collections.Generic;

namespace DesktopUI.Library.Models
{
    public class FollowersEnvelope
    {
        public List<Profile> Followers { get; set; }
        public int FollowersCount { get; set; }
    }
}