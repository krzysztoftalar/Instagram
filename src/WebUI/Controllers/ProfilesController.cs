using API.Controllers;
using Application.Profiles;
using Application.Profiles.Queries.Details;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class ProfilesController : BaseController
    {
        [HttpGet("{displayname}")]
        public async Task<ActionResult<Profile>> Details(string displayname)
        {
            return await Mediator.Send(new ProfileDetailsQuery { DisplayName = displayname });
        }
    }
}
