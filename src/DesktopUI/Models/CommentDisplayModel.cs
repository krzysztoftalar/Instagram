using System;

namespace DesktopUI.Models
{
    public class CommentDisplayModel
    {
        public Guid CommentId { get; set; }
        public string PhotoId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }

        public string Date { get; set; }
        public bool IsLoggedInComment { get; set; }
    }
}