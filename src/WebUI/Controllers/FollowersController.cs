using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Followers.Commands.Add;
using Application.Services.Followers.Commands.Delete;
using Application.Services.Followers.Queries.List;
using Application.Services.Profiles;

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
        public async Task<ActionResult<List<Profile>>> List(string username, string predicate)
        {
            return await Mediator.Send(new FollowersListQuery { Username = username, Predicate = predicate });
        }
    }
}
