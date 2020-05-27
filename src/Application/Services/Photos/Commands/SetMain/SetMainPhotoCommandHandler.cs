using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Photos.Commands.SetMain
{
    public class SetMainPhotoCommandHandler : IRequestHandler<SetMainPhotoCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public SetMainPhotoCommandHandler(IApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await _context.Users
                .Include(x => x.Photos)
                .Where(x => x.UserName == _userAccessor.GetCurrentUsername())
                .SelectMany(x => x.Photos)
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var currentMainPhoto = await _context.Users
                .Include(x => x.Photos)
                .Where(x => x.UserName == _userAccessor.GetCurrentUsername())
                .SelectMany(x => x.Photos)
                .Where(x => x.IsMain)
                .FirstOrDefaultAsync(cancellationToken);

            if (photo == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {Photo = "Not Found"});
            }

            if (currentMainPhoto == photo) return Unit.Value;

            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }

            photo.IsMain = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}