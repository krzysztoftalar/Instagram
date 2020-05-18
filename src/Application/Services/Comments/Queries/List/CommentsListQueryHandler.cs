using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Comments.Queries.List
{
    public class CommentsListQueryHandler : IRequestHandler<CommentsListQuery, CommentsEnvelope>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentsListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentsEnvelope> Handle(CommentsListQuery request, CancellationToken cancellationToken)
        {
            var queryable = _context.Photos
              .Include(x => x.Comments)
              .ThenInclude(x => x.Author)
              .Where(x => x.Id == request.PhotoId)
              .SelectMany(x => x.Comments)
              .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
              .OrderByDescending(x => x.CreatedAt)
              .AsQueryable();

            var comments = await PagedList<CommentDto>
              .CreateAsync(queryable, request.Skip, request.Limit);

            return new CommentsEnvelope
            {
                Comments = comments,
                CommentsCount = comments.TotalCount
            };
        }
    }
}
