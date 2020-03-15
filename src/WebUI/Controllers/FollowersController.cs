using API.Controllers;
using Application.Followers.Commands.Add;
using Application.Followers.Commands.Delete;
using Application.Followers.Queries.List;
using Application.Profiles;
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
            return await Mediator.Send(new AddFollwersCommand { Username = username });
        }

        [HttpDelete("{username}/follow")]
        public async Task<ActionResult<Unit>> UnFollow(string username)
        {
            return await Mediator.Send(new DeleteFollowersCommand { Username = username });
        }

        [HttpGet("{username}/follow")]
        public async Task<ActionResult<List<Profile>>> List(string username, string predicate)
        {
            return await Mediator.Send(new FollowersListQuery { Username = username, Predicate = predicate });
        }
    }
}
