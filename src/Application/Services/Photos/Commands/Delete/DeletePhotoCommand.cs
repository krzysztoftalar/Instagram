using MediatR;

namespace Application.Services.Photos.Commands.Delete
{
    public class DeletePhotoCommand : IRequest
    {
        public string Id { get; set; }
    }
}