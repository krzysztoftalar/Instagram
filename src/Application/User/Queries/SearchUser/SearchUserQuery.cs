using MediatR;

namespace Application.User.Queries.SearchUser
{
    public class SearchUserQuery : IRequest<UserDto>
    {
        public string DisplayName { get; set; }
    }
}
