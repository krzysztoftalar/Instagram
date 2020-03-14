using API.Controllers;
using Application.Profiles;
using Application.Profiles.Queries.Details;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
