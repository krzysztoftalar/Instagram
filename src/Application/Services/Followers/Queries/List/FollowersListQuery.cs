using MediatR;

namespace Application.Services.Followers.Queries.List
{
    public class FollowersListQuery : IRequest<FollowersEnvelope>
    {
        public string Username { get; set; }
        public string Predicate { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
    }
}