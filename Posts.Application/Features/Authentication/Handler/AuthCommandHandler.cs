using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostsProject.Application.Features.Authentication.Models;
using PostsProject.Application.Features.Authentication.Responses;
using PostsProject.Application.ResponseBase;
using PostsProject.Application.ServiceInterfaces;
using PostsProject.Data.Identity;

namespace PostsProject.Application.Features.Authentication.Handler
{
    public class AuthCommandHandler(UserManager<User> _userManager, IMapper _mapper, IAuthenticationService _authenticationService) : ResponseHandler,
        IRequestHandler<RegisterUserCommand, Response<JwtAuthResponse>>,
        IRequestHandler<LoginUserCommand, Response<JwtAuthResponse>>,
        IRequestHandler<RevokeRefreshTokenCommand, Response<string>>,
        IRequestHandler<RenewTokensCommand, Response<JwtAuthResponse>>
    {
        public async Task<Response<JwtAuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join("\n", createResult.Errors.Select(e => e.Description));
                return BadRequest<JwtAuthResponse>(errors);
            }

            await _userManager.AddToRoleAsync(user, "User");


            var jwtAuthResponse = await _authenticationService.LoginUser(user);

            return Success(jwtAuthResponse, message: $"User with username: {user.UserName} registered successfully.");

        }
        public async Task<Response<JwtAuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return BadRequest<JwtAuthResponse>("Invalid username or password.");

            var jwtAuthResponse = await _authenticationService.LoginUser(user);

            return Success(jwtAuthResponse, message: $"User with username: {user.UserName} logged in successfully.");
        }
        public async Task<Response<string>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (await _authenticationService.RevokeRefreshTokenAsync(request.RefreshToken))
                return Success("Refresh token revoked successfully.");

            else
                return BadRequest<string>("Refresh token is invalid, No revoke!");
        }
        public async Task<Response<JwtAuthResponse>> Handle(RenewTokensCommand request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.RenewTokensAsync(request.RefreshToken);
            if (response == null)
                return BadRequest<JwtAuthResponse>("Invalid token");

            return Success(response);
        }
    }
}
