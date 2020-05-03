using System.Collections.Generic;

namespace Application.Services.Followers.Queries.List
{
    public class FollowersEnvelope
    {
        public List<FollowerDto> Followers { get; set; }
        public int FollowersCount { get; set; }
    }
}