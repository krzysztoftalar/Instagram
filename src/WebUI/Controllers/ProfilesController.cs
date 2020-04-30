using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services.Profiles;
using Application.Services.Profiles.Queries.Details;

namespace WebUI.Controllers
{
    public class ProfilesController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Details(string username)
        {
            return await Mediator.Send(new ProfileDetailsQuery { Username = username });
        }
    }
}
