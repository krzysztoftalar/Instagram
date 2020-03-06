using MediatR;

namespace Application.Profiles.Queries.Details
{
    public class ProfileDetailsQuery : IRequest<Profile>
    {
        public string DisplayName { get; set; }
    }
}
