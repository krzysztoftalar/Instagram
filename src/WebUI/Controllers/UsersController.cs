using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.User.Commands.Register;
using Application.Services.User.Queries.CurrentUser;
using Application.Services.User.Queries.Login;
using Application.Services.User.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class UsersController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserDto>> Register(RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginUserDto>> Login(LoginUserQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet]
        public async Task<ActionResult<CurrentUserDto>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUserQuery());
        }

        [HttpGet("{displayName}")]
        public async Task<ActionResult<List<SearchUserDto>>> UsersList(string displayName)
        {
            return await Mediator.Send(new SearchUsersQuery {DisplayName = displayName});
        }
    }
}