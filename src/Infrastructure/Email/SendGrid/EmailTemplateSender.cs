using Application.Errors;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email.SendGrid
{
    public class EmailTemplateSender : IEmailTemplateSender
    {
        private readonly ISendGridEmailSender _emailSender;

        public EmailTemplateSender(ISendGridEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<ResponseError> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            string templateText;

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Infrastructure.Email.Templates.GeneralTemplate.htm")!, Encoding.UTF8))
            {
                templateText = await reader.ReadToEndAsync();
            }

            templateText = templateText
                .Replace("--Title--", title)
                .Replace("--Content1--", content1)
                .Replace("--Content2--", content2)
                .Replace("--ButtonText--", buttonText)
                .Replace("--ButtonUrl--", buttonUrl);

            details.Content = templateText;

            return await _emailSender.SendEmailAsync(details);
        }
    }
}