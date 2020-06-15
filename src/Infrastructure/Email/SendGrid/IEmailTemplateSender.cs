using System.Threading.Tasks;
using Application.Errors;

namespace Infrastructure.Email.SendGrid
{
    public interface IEmailTemplateSender
    {
        Task<ResponseError> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2,
            string buttonText, string buttonUrl);
    }
}