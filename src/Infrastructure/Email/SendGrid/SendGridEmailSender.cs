using Application.Errors;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Email.SendGrid
{
    public class SendGridEmailSender : ISendGridEmailSender
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseError> SendEmailAsync(SendEmailDetails details)
        {
            var apiKey = _configuration["SendGrid:SendGridKey"];

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(details.FromEmail, details.FromName);

            var to = new EmailAddress(details.ToEmail, details.ToName);

            var subject = details.Subject;

            var message = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                details.IsHtml ? null : details.Content,
                details.IsHtml ? details.Content : null);

            var response = await client.SendEmailAsync(message);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return new ResponseError();
            }
            else
            {
                var bodyResult = await response.Body.ReadAsStringAsync();

                var sendEmailResponse = JsonConvert.DeserializeObject<ResponseError>(bodyResult);

                return sendEmailResponse;
            }
        }
    }
}