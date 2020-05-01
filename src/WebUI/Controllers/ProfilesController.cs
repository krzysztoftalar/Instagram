using Application.Services.Profiles.Commands.Edit;
using Application.Services.Profiles.Queries.Details;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class ProfilesController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileDto>> Details(string username)
        {
            return await Mediator.Send(new ProfileDetailsQuery { Username = username });
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Edit(EditProfileCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
