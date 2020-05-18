using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Linq;

namespace Application.Services.Comments.Queries.List
{
    public class CommentDto : IMapFrom<Comment>
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Comment, CommentDto>()
              .ForMember(d => d.DisplayName,
                opt =>
                  opt.MapFrom(src => src.Author.DisplayName))
              .ForMember(d => d.Username,
                opt =>
                  opt.MapFrom(src => src.Author.UserName))
              .ForMember(d => d.Image,
                opt =>
                  opt.MapFrom(src => src.Author.Photos
                    .FirstOrDefault(x => x.IsMain).Url));
        }
    }
}