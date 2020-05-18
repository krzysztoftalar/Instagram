using MediatR;

namespace Application.Services.Comments.Commands.Create
{
    public class CreateCommentCommand : IRequest<CommentDto>
    {
        public string PhotoId { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
    }
}
