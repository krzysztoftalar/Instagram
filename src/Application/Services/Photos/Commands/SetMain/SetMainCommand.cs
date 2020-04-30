using MediatR;

namespace Application.Services.Photos.Commands.SetMain
{
    public class SetMainCommand : IRequest
    {
        public string Id { get; set; }
    }
}