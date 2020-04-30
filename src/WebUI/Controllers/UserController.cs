using API.Controllers;
using Application.User;
using Application.User.Commands.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.User;
using Application.Services.User.Commands.Register;
using Application.Services.User.Queries.CurrentUser;
using Application.Services.User.Queries.Login;
using Application.Services.User.Queries.Search;

namespace WebUI.Controllers
{
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUserQuery());
        }

        [HttpGet("{displayname}")]
        public async Task<ActionResult<List<UserDto>>> UsersList(string displayname)
        {
            return await Mediator.Send(new SearchUsersQuery { DisplayName = displayname });
        }
    }
}
