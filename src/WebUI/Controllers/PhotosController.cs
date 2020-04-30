using API.Controllers;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services.Photos.Commands.Add;
using Application.Services.Photos.Commands.Delete;
using Application.Services.Photos.Commands.SetMain;
using MediatR;

namespace WebUI.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm] AddPhotoCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("{id}/setMain")]
        public async Task<ActionResult<Unit>> SetMain(string id)
        {
            return await Mediator.Send(new SetMainCommand {Id = id});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await Mediator.Send(new DeletePhotoCommand {Id = id});
        }
    }
}