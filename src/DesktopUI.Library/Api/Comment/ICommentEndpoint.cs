using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Comment
{
    public interface ICommentEndpoint
    {
        Task AddCommentAsync(Models.DbModels.Comment comment);
        Task<CommentsEnvelope> LoadCommentsAsync(string id, int? skip, int? limit);
    }
}
