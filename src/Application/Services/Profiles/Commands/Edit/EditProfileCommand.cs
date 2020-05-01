using MediatR;

namespace Application.Services.Profiles.Commands.Edit
{
    public class EditProfileCommand : IRequest
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}
