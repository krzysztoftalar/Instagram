using FluentValidation;

namespace Application.Services.Profiles.Commands.Edit
{
    public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileCommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}
