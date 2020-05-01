using System.Linq;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.User.Queries.Search
{
    public class SearchUserDto : IMapFrom<Photo>
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, SearchUserDto>()
                .ForMember(d =>
                    d.Image, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}