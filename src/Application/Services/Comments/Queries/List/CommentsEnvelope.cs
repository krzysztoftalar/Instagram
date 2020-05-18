using Application.Services.Photos.Queries.List;
using System.Collections.Generic;

namespace Application.Services.Comments.Queries.List
{
    public class CommentsEnvelope
    {
        public List<CommentDto> Comments { get; set; }
        public int CommentsCount { get; set; }
    }
}
