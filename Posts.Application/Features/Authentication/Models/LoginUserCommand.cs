using PostsProject.Application.Features.Authentication.Responses;
using PostsProject.Application.ResponseBase;
using MediatR;

namespace PostsProject.Application.Features.Authentication.Models
{
    public class LoginUserCommand : IRequest<Response<JwtAuthResponse>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
