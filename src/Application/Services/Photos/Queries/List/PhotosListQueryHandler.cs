using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Photos.Queries.List
{
    public class PhotosListQueryHandler : IRequestHandler<PhotosListQuery, PhotosEnvelope>
    {
        private readonly IApplicationDbContext _context;

        public PhotosListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PhotosEnvelope> Handle(PhotosListQuery request, CancellationToken cancellationToken)
        {
            var queryable = _context.Users
                .Include(x => x.Photos)
                .Where(x => x.UserName == request.Username)
                .SelectMany(x => x.Photos.Select(y => new PhotoDto
                    {
                        Id = y.Id,
                        Url = y.Url,
                        IsMain = y.IsMain
                    })
                    .AsQueryable());

            return new PhotosEnvelope
            {
                Photos = await queryable
                    .Skip(request.Skip ?? 0)
                    .Take(request.Limit ?? 10)
                    .ToListAsync(cancellationToken),
                PhotosCount = await queryable.CountAsync(cancellationToken)
            };
        }
    }
}