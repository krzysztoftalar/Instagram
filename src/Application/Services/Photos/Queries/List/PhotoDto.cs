using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Photos.Queries.List
{
    public class PhotoDto : IMapFrom<Photo>
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Photo, PhotoDto>();
        }
    }
}