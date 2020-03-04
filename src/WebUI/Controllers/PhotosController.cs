using API.Controllers;
using Application.Photos.Command;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm]AddPhotoCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}