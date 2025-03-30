using FluentValidation;
using PostsProject.Application.Features.Authentication.Models;

namespace PostsProject.Application.Features.Authentication.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            ApplyValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name can't be empty!")
                .WithErrorCode("400");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} can't be empty!")
                .WithErrorCode("400");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User name can't be empty!")
                .WithErrorCode("400");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("{PropertyName} can't be empty!")
                .WithErrorCode("400");

            RuleFor(x => x.ConfirmPassword)
               .NotEmpty().WithMessage("Confirm Password can't be empty!")
               .WithErrorCode("400")
               .Equal(x => x.Password).WithMessage("Password and confirm password don't matched!")
               .WithErrorCode("400");

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^\+?[0-9]{7,15}$")
                .WithMessage("Invalid phone number format.")
                .WithErrorCode("400");

        }
    }
}
