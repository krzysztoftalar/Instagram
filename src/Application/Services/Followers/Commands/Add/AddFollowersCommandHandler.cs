using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Followers.Commands.Add
{
    public class AddFollowersCommandHandler : IRequestHandler<AddFollowersCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IApplicationDbContext _context;

        public AddFollowersCommandHandler(IUserAccessor userAccessor, IApplicationDbContext context)
        {
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Unit> Handle(AddFollowersCommand request, CancellationToken cancellationToken)
        {
            var observer = await _context.Users
                .Where(x => x.UserName == _userAccessor.GetCurrentUsername())
                .Select(x => new {x.Id})
                .SingleOrDefaultAsync(cancellationToken);

            var target = await _context.Users
                .Where(x => x.UserName == request.Username)
                .Select(x => new {x.Id})
                .SingleOrDefaultAsync(cancellationToken);

            if (target == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {User = "Not Found"});
            }

            if (target.Id == observer.Id)
            {
                throw new RestException(HttpStatusCode.BadRequest, new {User = "You can not following yourself"});
            }

            var following = await _context.Followings.SingleOrDefaultAsync(x =>
                x.ObserverId == observer.Id && x.TargetId == target.Id, cancellationToken: cancellationToken);

            if (following != null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new {User = "You are already following this user"});
            }

            following = new UserFollowing
            {
                ObserverId = observer.Id,
                TargetId = target.Id
            };

            await _context.Followings.AddAsync(following, cancellationToken);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}