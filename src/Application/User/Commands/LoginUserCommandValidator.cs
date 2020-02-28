using Application.Validators;
using FluentValidation;

namespace Application.User.Commands
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).Password();
        }
    }
}
