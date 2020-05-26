using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Photos.Commands.Delete
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public DeletePhotoCommandHandler(IApplicationDbContext context, IPhotoAccessor photoAccessor,
            IUserAccessor userAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await _context.Photos
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (photo == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not Found" });
            }

            var result = _photoAccessor.DeletePhoto(request.Id);

            if (result == null)
            {
                throw new Exception("Problem deleting the photo");
            }

            _context.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}