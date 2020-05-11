using MediatR;

namespace Application.Services.Photos.Commands.SetMain
{
    public class SetMainPhotoCommand : IRequest
    {
        public string Id { get; set; }
    }
}