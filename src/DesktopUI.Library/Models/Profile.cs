using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DesktopUI.Library.Models
{
    public class Profile
    {
        public Profile()
        {
            Photos = new Collection<Photo>();
        }

        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
