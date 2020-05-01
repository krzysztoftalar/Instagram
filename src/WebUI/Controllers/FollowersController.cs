using Application.Services.Followers.Commands.Add;
using Application.Services.Followers.Commands.Delete;
using Application.Services.Followers.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/profiles")]
    public class FollowersController : BaseController
    {
        [HttpPost("{username}/follow")]
        public async Task<ActionResult<Unit>> Follow(string username)
        {
            return await Mediator.Send(new AddFollowersCommand { Username = username });
        }

        [HttpDelete("{username}/follow")]
        public async Task<ActionResult<Unit>> UnFollow(string username)
        {
            return await Mediator.Send(new DeleteFollowersCommand { Username = username });
        }

        [HttpGet("{username}/follow")]
        public async Task<ActionResult<List<ProfileDto>>> List(string username, string predicate)
        {
            return await Mediator.Send(new FollowersListQuery { Username = username, Predicate = predicate });
        }
    }
}
