using System;

namespace DesktopUI.Library.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string PhotoId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }

        public bool IsLoggedInComment { get; set; }
    }
}
