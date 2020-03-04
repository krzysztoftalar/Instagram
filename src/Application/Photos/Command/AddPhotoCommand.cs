using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Photos.Command
{
    public class AddPhotoCommand : IRequest<Photo>
    {
        public IFormFile File { get; set; }
    }
}