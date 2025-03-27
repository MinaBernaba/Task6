using MediatR;
using PostsProject.Application.ResponseBase;

namespace Posts.Application.Features.Posts.Commands.Models
{
    public class CreatePostCommand : IRequest<Response<string>>
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
