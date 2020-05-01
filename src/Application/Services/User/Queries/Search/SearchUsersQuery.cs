using System.Collections.Generic;
using MediatR;

namespace Application.Services.User.Queries.Search
{
    public class SearchUsersQuery : IRequest<List<SearchUserDto>>
    {
        public string DisplayName { get; set; }
    }
}
