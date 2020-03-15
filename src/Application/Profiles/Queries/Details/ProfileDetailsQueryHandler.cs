using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Profiles.Queries.Details
{
    public class ProfileDetailsQueryHandler : IRequestHandler<ProfileDetailsQuery, Profile>
    {
        private readonly IProfileReader _profileReader;

        public ProfileDetailsQueryHandler(IProfileReader profileReader)
        {
            _profileReader = profileReader;
        }

        public async Task<Profile> Handle(ProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _profileReader.ReadProfile(request.Username);
        }
    }
}
