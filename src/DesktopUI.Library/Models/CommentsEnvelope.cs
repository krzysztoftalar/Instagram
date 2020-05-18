using System.Collections.Generic;

namespace DesktopUI.Library.Models
{
    public class CommentsEnvelope
    {
        public List<Comment> Comments { get; set; }
        public int CommentsCount { get; set; }
    }
}
