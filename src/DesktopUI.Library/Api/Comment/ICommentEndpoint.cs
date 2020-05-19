using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Comment
{
    public interface ICommentEndpoint
    {
        Task AddComment(Models.DbModels.Comment comment);
        Task<CommentsEnvelope> LoadComments(string id, int? skip, int? limit);
    }
}
