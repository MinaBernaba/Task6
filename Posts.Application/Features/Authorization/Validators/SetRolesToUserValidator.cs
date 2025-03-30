using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostsProject.Application.Features.Authorization.Models;
using PostsProject.Data.Identity;

namespace PostsProject.Application.Features.Authorization.Validators
{
    public class SetRolesToUserValidator : AbstractValidator<SetRolesToUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SetRolesToUserValidator(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            ApplyValidationRules();
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<SetRolesToUserCommand> context, CancellationToken cancellation = default)
        {
            var command = context.InstanceToValidate;
            var userExistsTask = await _userManager.Users.AnyAsync(u => u.Id == command.UserId, cancellation);
            var allRolesTask = await _roleManager.Roles.Select(r => r.Name).ToListAsync(cancellation);

            context.RootContextData["UserExists"] = userExistsTask;
            context.RootContextData["ExistingRoles"] = allRolesTask;

            return await base.ValidateAsync(context, cancellation);
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.UserId)
                        .Must((_, _, context) => (bool)context.RootContextData["UserExists"])
                        .WithMessage(x => $"User with ID: {x.UserId} does not exist.")
                        .WithErrorCode("404");
                });

            RuleFor(x => x.Roles)
                .NotEmpty()
                .WithMessage("Roles list can't be empty.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleForEach(x => x.Roles)
                        .NotEmpty()
                        .WithMessage("Each role must be a valid non-empty string.")
                        .WithErrorCode("400")
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Roles)
                            .Must((command, roles, context) =>
                            {
                                var existingRoles = context.RootContextData["ExistingRoles"] as List<string>;
                                return existingRoles != null && roles.All(role => existingRoles.Contains(role));
                            })
                            .WithMessage("One or more roles do not exist.")
                            .WithErrorCode("404");
                        });
                });
        }
    }
}