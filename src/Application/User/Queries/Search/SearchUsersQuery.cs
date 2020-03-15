using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.User.Queries.Search
{
    public class SearchUsersQuery : IRequest<List<UserDto>>
    {
        public string DisplayName { get; set; }
    }
}
