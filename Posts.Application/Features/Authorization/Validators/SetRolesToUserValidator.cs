using PostsProject.Application.Features.Authorization.Models;
using PostsProject.Data.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Roles list is invalid!")
                .ForEach(role =>
                role.NotEmpty().WithMessage("Each role must be valid!")
                .Must(r => !string.IsNullOrWhiteSpace(r)).WithMessage("Each role must be a valid!")
                );
        }

        public void ApplyCustomValidationRules()
        {

            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) => await _userManager.Users.AnyAsync(u => u.Id == userId))
                .WithMessage(x => $"User with ID: {x.UserId} does not exist.");

            RuleFor(x => x.Roles)
            .MustAsync(async (roles, cancellation) =>
            {
                var existingRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return roles.All(role => existingRoles.Contains(role));
            })
            .WithMessage("One or more roles do not exist.");
        }
    }

}