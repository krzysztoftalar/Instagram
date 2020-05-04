using System.Linq;
using Application.Mappings;
using Domain.Entities;

namespace Application.Services.Profiles.Queries.Details
{
    public class ProfileDto : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<AppUser, ProfileDto>()
                .ForMember(d =>
                    d.Image, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d =>
                    d.FollowingCount, opt =>
                    opt.MapFrom(src => src.Followings.Count()))
                .ForMember(d =>
                    d.FollowersCount, opt =>
                    opt.MapFrom(src => src.Followers.Count()));
        }
    }
}