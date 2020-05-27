using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Comments.Commands.Delete
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public DeleteCommentCommandHandler(IApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .SingleOrDefaultAsync(x => x.CommentId == request.Id, cancellationToken);

            var user = await _context.Users
                .Where(x => x.UserName == _userAccessor.GetCurrentUsername())
                .Select(x => new {x.Id})
                .SingleOrDefaultAsync(cancellationToken);

            if (comment == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {Comment = "Not Found"});
            }

            if (comment.AuthorId != user.Id)
            {
                throw new RestException(HttpStatusCode.BadRequest, new {Comment = "You can not remove this comment"});
            }

            _context.Comments.Remove(comment);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}