using System.Collections.Generic;

namespace DesktopUI.Library.Models
{
    public class PhotosEnvelope
    {
        public List<Photo> Photos { get; set; }
        public int PhotosCount { get; set; }
    }
}