using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services.Photos.Commands.Add;
using Application.Services.Photos.Commands.Delete;
using Application.Services.Photos.Commands.SetMain;
using Application.Services.Photos.Queries.List;
using MediatR;
using PhotoDto = Application.Services.Photos.Commands.Add.PhotoDto;

namespace WebUI.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<PhotoDto>> Add([FromForm] AddPhotoCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("{id}/setMain")]
        public async Task<ActionResult<Unit>> SetMain(string id)
        {
            return await Mediator.Send(new SetMainPhotoCommand {Id = id});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await Mediator.Send(new DeletePhotoCommand {Id = id});
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<PhotosEnvelope>> List(string username, int? skip, int? limit)
        {
            return await Mediator.Send(new PhotosListQuery {Username = username, Skip = skip, Limit = limit});
        }
    }
}