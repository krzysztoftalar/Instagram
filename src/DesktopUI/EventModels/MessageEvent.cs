using DesktopUI.Library.Models;
using System.Collections.Generic;

namespace DesktopUI.EventModels
{
    public class MessageEvent
    {
        public string Message { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
