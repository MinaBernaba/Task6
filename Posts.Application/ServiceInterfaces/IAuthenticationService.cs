using PostsProject.Application.Features.Authentication.Responses;
using PostsProject.Data.Identity;

namespace PostsProject.Application.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<JwtAuthResponse> LoginUser(User user);
        Task<JwtAuthResponse?> RenewTokensAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
