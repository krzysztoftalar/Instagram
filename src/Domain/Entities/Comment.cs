using System;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public string AuthorId { get; set; }
        public virtual AppUser Author { get; set; }

        public string PhotoId { get; set; }
        public virtual Photo Photo { get; set; }
    }
}
