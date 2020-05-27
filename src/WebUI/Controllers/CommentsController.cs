using System;
using Application.Services.Comments.Queries.List;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services.Comments.Commands.Delete;
using MediatR;

namespace WebUI.Controllers
{
    public class CommentsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentsEnvelope>> List(string id, int? skip, int? limit)
        {
            return await Mediator.Send(new CommentsListQuery {PhotoId = id, Skip = skip, Limit = limit});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteCommentCommand {Id = id});
        }
    }
}