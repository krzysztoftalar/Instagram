using MediatR;

namespace Application.Services.Followers.Queries.List
{
    public class FollowersListQuery 
    {
        public class Query : IRequest<FollowersEnvelope>
        {
            public Query(string username, string predicate, int? skip, int? limit)
            {
                Username = username;
                Predicate = predicate;
                Skip = skip;
                Limit = limit;
            }

            public string Username { get; set; }
            public string Predicate { get; set; }
            public int? Skip { get; set; }
            public int? Limit { get; set; }
        }
    }
}