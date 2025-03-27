using MediatR;
using Posts.Application.Features.Posts.Queries.Models;
using Posts.Application.Features.Posts.Queries.Response;
using Posts.Application.ServiceInterfaces;
using PostsProject.Application.ResponseBase;

namespace Posts.Application.Features.Posts.Queries.Handler
{
    public class PostsQueryHandler(IPostService _postService) : ResponseHandler,
        IRequestHandler<GetAllApprovedPostsQuery, Response<IEnumerable<GetAllApprovedPostsResponse>>>,
        IRequestHandler<GetAllPostsQuery, Response<IEnumerable<GetAllPostsResponse>>>,
        IRequestHandler<GetPostByIdQuery, Response<GetPostInfoResponse>>
    {
        public async Task<Response<IEnumerable<GetAllApprovedPostsResponse>>> Handle(GetAllApprovedPostsQuery request, CancellationToken cancellationToken)
        {
            var response = await _postService.GetAllApprovedPostsAsync();
            return Success(response);
        }

        public async Task<Response<IEnumerable<GetAllPostsResponse>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var response = await _postService.GetAllPostsAsync();
            return Success(response);
        }

        public async Task<Response<GetPostInfoResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _postService.GetPostWithDetailsByIdAsync(request.PostId);
            return Success(response);
        }
    }
}
