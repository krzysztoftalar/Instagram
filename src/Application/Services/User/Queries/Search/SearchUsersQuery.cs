using System.Collections.Generic;
using MediatR;

namespace Application.Services.User.Queries.Search
{
    public class SearchUsersQuery : IRequest<List<UserDto>>
    {
        public string DisplayName { get; set; }
    }
}
