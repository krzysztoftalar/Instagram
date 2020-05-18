using MediatR;

namespace Application.Services.Comments.Queries.List
{
    public class CommentsListQuery : IRequest<CommentsEnvelope>
    {
        public string PhotoId { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
    }
}
