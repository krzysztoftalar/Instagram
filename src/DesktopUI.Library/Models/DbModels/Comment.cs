using System;

namespace DesktopUI.Library.Models.DbModels
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string PhotoId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Date { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }

        public bool IsLoggedInComment { get; set; }
    }
}
