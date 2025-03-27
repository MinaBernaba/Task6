using PostsProject.Application.Features.Authorization.Models;
using PostsProject.Application.ResponseBase;
using PostsProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PostsProject.Application.Features.Authorization.Handler
{
    public class AuthorizationCommandHandler(UserManager<User> _userManager) : ResponseHandler,
        IRequestHandler<SetRolesToUserCommand, Response<string>>
    {
        public async Task<Response<string>> Handle(SetRolesToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == request.UserId);

            var result = await _userManager.AddToRolesAsync(user, request.Roles);

            if (!result.Succeeded)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                return BadRequest<string>(errors);
            }

            return Success($"Roles added successfully to the given user ID: {user.Id}.");
        }
    }
}
