using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Profiles.Commands.Edit
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IApplicationDbContext _context;

        public EditProfileCommandHandler(IUserAccessor userAccessor, IApplicationDbContext context)
        {
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername(), cancellationToken);

            if (user.DisplayName == request.DisplayName && user.Bio == request.Bio) return Unit.Value;

            user.DisplayName = request.DisplayName ?? user.DisplayName;
            user.Bio = request.Bio ?? user.Bio;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
