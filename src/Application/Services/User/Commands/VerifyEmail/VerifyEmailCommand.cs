using MediatR;

namespace Application.Services.User.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest
    {
        public string UserId { get; set; }
        public string EmailToken { get; set; }
    }
}