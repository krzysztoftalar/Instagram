using FluentValidation;

namespace Application.Services.Comments.Commands.Create
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}