using Application.Services.Comments.Queries.List;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class CommentsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentsEnvelope>> List(string id, int? skip, int? limit)
        {
            return await Mediator.Send(new CommentsListQuery { PhotoId = id, Skip = skip, Limit = limit });
        }
    }
}
