using MediatR;
using PostsProject.Application.ResponseBase;

namespace Posts.Application.Features.Posts.Commands.Models
{
    public class UpdatePostCommand : IRequest<Response<string>>
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
