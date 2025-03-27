using PostsProject.Application.ResponseBase;
using MediatR;

namespace PostsProject.Application.Features.Authentication.Models
{
    public class RevokeRefreshTokenCommand : IRequest<Response<string>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
