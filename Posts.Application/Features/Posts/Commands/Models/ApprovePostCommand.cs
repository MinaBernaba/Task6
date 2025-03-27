using MediatR;
using PostsProject.Application.ResponseBase;

namespace Posts.Application.Features.Posts.Commands.Models
{
    public class ApprovePostCommand : IRequest<Response<string>>
    {
        public int PostId { get; set; }
    }
}
