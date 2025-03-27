using PostsProject.Application.Features.Authentication.Responses;
using PostsProject.Application.ResponseBase;
using MediatR;

namespace PostsProject.Application.Features.Authentication.Models
{
    public class RenewTokensCommand : IRequest<Response<JwtAuthResponse>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
