using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Photos.Commands.Add
{
    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, Photo>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public AddPhotoCommandHandler(IApplicationDbContext context, IPhotoAccessor photoAccessor,
            IUserAccessor userAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }
        
        public async Task<Photo> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var photoUploadResult = _photoAccessor.AddPhoto(request.File);

            var user = await _context.Users.SingleOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

            var photo = new Photo
            {
                Id = photoUploadResult.PublicId,
                Url = photoUploadResult.Url
            };

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return photo;

            throw new Exception("Problem saving changes");
        }
    }
}