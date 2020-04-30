using MediatR;

namespace Application.Services.Profiles.Queries.Details
{
    public class ProfileDetailsQuery : IRequest<Profile>
    {
        public string Username { get; set; }
    }
}
