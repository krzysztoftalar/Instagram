using API.Controllers;
using Application.User;
using Application.User.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(LoginUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
