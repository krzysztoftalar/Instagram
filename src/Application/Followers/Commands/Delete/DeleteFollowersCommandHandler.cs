using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Followers.Commands.Delete
{
    public class DeleteFollowersCommandHandler : IRequestHandler<DeleteFollowersCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public DeleteFollowersCommandHandler(IApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeleteFollowersCommand request, CancellationToken cancellationToken)
        {
            var observer = await _context.Users.SingleOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetCurrentUsername());

            var target = await _context.Users.SingleOrDefaultAsync(x =>
                x.UserName == request.Username);

            if (target == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not Found" });
            }

            var following = await _context.Followings.SingleOrDefaultAsync(x =>
                x.ObserverId == observer.Id && x.TargetId == target.Id);

            if (following == null)
            {
                throw new RestException(HttpStatusCode.BadRequest,
                   new { User = "You are not following this user" });
            }               

            if (following != null)
            {
                _context.Followings.Remove(following);
            }

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
