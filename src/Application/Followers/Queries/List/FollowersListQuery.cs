using Application.Profiles;
using MediatR;
using System.Collections.Generic;

namespace Application.Followers.Queries.List
{
    public class FollowersListQuery : IRequest<List<Profile>>
    {
        public string Username { get; set; }
        public string Predicate { get; set; }
    }
}
