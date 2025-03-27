using MediatR;
using Posts.Application.Features.Posts.Queries.Response;
using PostsProject.Application.ResponseBase;

namespace Posts.Application.Features.Posts.Queries.Models
{
    public class GetAllPostsQuery : IRequest<Response<IEnumerable<GetAllPostsResponse>>>
    {
    }
}
