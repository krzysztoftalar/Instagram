using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Photos.Commands.Add
{
    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, PhotoDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public AddPhotoCommandHandler(IApplicationDbContext context, IPhotoAccessor photoAccessor,
            IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<PhotoDto> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var photoUploadResult = _photoAccessor.AddPhoto(request.File);

            if (photoUploadResult == null)
            {
                throw new Exception("Problem uploading the photo");
            }

            var user = await _context.Users
                .Include(x => x.Photos)
                .Where(x => x.UserName == _userAccessor.GetCurrentUsername())
                .Select(x => new { UserId = x.Id, MainPhoto = x.Photos.FirstOrDefault(y => y.IsMain) })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            var photo = new Photo
            {
                Id = photoUploadResult.PublicId,
                Url = photoUploadResult.Url,
                AppUserId = user.UserId
            };

            if (user.MainPhoto == null)
            {
                photo.IsMain = true;
            }

            await _context.Photos.AddAsync(photo, cancellationToken);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success) return _mapper.Map<PhotoDto>(photo);

            throw new Exception("Problem saving changes");
        }
    }
}