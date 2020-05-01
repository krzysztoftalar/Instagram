using Application.Mappings;
using Domain.Entities;

namespace Application.Services.Profiles.Queries.Details
{
    public class PhotoDto : IMapFrom<Photo>
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Photo, PhotoDto>();
        }
    }
}