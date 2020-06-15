using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Email.SendGrid;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Email
{
    public class EmailAccessor : IEmailAccessor
    {
        private readonly UserManager<AppUser> _userManager;
        private static IOptions<EmailSettings> _config;
        private readonly IEmailTemplateSender _emailSender;
        private readonly IConfiguration _configuration;

        public EmailAccessor(UserManager<AppUser> userManager, IOptions<EmailSettings> config,
            IEmailTemplateSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _config = config;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<ResponseError> SendUserEmailVerificationAsync(AppUser user)
        {
            var userIdentity = await _userManager.FindByNameAsync(user.UserName);

            var emailVerificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var verificationUrl =
                $"{_configuration["AppSettings:Url"]}/api/users/verify/email?userId={HttpUtility.UrlEncode(userIdentity.Id)}&emailToken={HttpUtility.UrlEncode(emailVerificationCode)}";

            return await _emailSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                IsHtml = true,
                FromEmail = _config.Value.SendEmailFromEmail,
                FromName = _config.Value.SendEmailFromName,
                ToEmail = userIdentity.Email,
                ToName = user.UserName,
                Subject = "Verify Your Email - Instagram Team"
            },
                "Verify Email",
                $"Hi {user.UserName ?? "stranger"},",
                "Thanks for creating an account with us.<br/>To continue please verify your email.",
                "Verify Email",
                verificationUrl
            );
        }
    }
}