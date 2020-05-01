using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Photos.Commands.Add
{
    public class AddPhotoCommand : IRequest<PhotoDto>
    {
        public IFormFile File { get; set; }
    }
}