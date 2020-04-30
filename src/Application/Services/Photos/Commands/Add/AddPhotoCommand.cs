using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Photos.Commands.Add
{
    public class AddPhotoCommand : IRequest<Photo>
    {
        public IFormFile File { get; set; }
    }
}