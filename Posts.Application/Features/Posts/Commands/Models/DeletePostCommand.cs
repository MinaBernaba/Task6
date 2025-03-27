using MediatR;
using PostsProject.Application.ResponseBase;

namespace Posts.Application.Features.Posts.Commands.Models
{
    public class DeletePostCommand : IRequest<Response<string>>
    {
        public int PostId { get; set; }
    }
}
