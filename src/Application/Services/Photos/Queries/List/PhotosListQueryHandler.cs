using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
              .AsNoTracking()
              .AsQueryable();

            var photos = await PagedList<PhotoDto>
              .CreateAsync(queryable, request.Skip, request.Limit);

            return new PhotosEnvelope
            {
                Photos = photos,
                PhotosCount = photos.TotalCount
            };
        }
    }
}