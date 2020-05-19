using System.Collections.Generic;
using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Library.Models
{
    public class CommentsEnvelope
    {
        public List<Comment> Comments { get; set; }
        public int CommentsCount { get; set; }
    }
}
