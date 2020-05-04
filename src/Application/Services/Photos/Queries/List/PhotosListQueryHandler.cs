using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Photos.Queries.List
{
    public class PhotosListQueryHandler : IRequestHandler<PhotosListQuery, PhotosEnvelope>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PhotosListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PhotosEnvelope> Handle(PhotosListQuery request, CancellationToken cancellationToken)
        {
            var queryable = _context.Users
                .Include(x => x.Photos)
                .Where(x => x.UserName == request.Username)
                .SelectMany(x => x.Photos)
                .ProjectTo<PhotoDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

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