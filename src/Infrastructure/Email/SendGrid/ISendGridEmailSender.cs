using System.Threading.Tasks;
using Application.Errors;

namespace Infrastructure.Email.SendGrid
{
    public interface ISendGridEmailSender
    {
        Task<ResponseError> SendEmailAsync(SendEmailDetails details);
    }
}