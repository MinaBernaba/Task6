using PostsProject.Application.ResponseBase;
using MediatR;

namespace PostsProject.Application.Features.Authorization.Models
{
    public class SetRolesToUserCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string[] Roles { get; set; } = null!;
    }
}
