using Application.Mappings;

namespace Application.Services.Profiles.Queries.Details
{
    public class ProfileDto : IMapFrom<Profile>
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Profile, ProfileDto>();
        }
    }
}