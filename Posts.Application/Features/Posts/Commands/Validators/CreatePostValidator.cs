using FluentValidation;
using Posts.Application.Features.Posts.Commands.Models;

namespace Posts.Application.Features.Posts.Commands.Validators
{
    public class CreatePostValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostValidator()
        {
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        private void ApplyValidationRules()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(30).WithMessage("Title must not exceed 30 characters.");
            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body is required.")
                .MaximumLength(2000).WithMessage("Body must not exceed 2000 characters.");
        }
        private void ApplyCustomValidationRules()
        {

        }
    }
}
