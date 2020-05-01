using MediatR;

namespace Application.Services.Profiles.Queries.Details
{
    public class ProfileDetailsQuery : IRequest<ProfileDto>
    {
        public string Username { get; set; }
    }
}
