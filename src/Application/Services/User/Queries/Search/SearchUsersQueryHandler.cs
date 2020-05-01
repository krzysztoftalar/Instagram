using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.User.Queries.Search
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<SearchUserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SearchUserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(x => x.DisplayName == request.DisplayName)
                .Include(x => x.Photos)
                .ProjectTo<SearchUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}