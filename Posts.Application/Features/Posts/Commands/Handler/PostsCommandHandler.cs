using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Posts.Application.Features.Posts.Commands.Models;
using Posts.Application.ServiceInterfaces;
using Posts.Data.Entities;
using PostsProject.Application.ResponseBase;
using System.Security.Claims;

namespace Posts.Application.Features.Posts.Commands.Handler
{
    public class PostsCommandHandler(IPostService _postService, IMapper _mapper, IHttpContextAccessor _httpContext) : ResponseHandler,
        IRequestHandler<CreatePostCommand, Response<string>>,
        IRequestHandler<ApprovePostCommand, Response<string>>,
        IRequestHandler<UpdatePostCommand, Response<string>>,
        IRequestHandler<DeletePostCommand, Response<string>>
    {
        public async Task<Response<string>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            int userId = int.Parse(userIdClaim.Value);

            var post = _mapper.Map<Post>(request);
            post.UserId = userId;
            post.IsPublic = false;


            bool isAdded = await _postService.CreatePostAsync(post);

            if (isAdded)
                return Created<string>($"Post with ID: {post.Id} belongs to user ID: {userId} created successfully.");
            else
                return BadRequest<string>("Failed to create post.");
        }

        public async Task<Response<string>> Handle(ApprovePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);

            post.IsPublic = true;

            if (await _postService.UpdatePostAsync(post))
                return Success($"Post with ID: {post.Id} approved successfully.");

            else
                return BadRequest<string>("Failed to approve post.");
        }

        public async Task<Response<string>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);

            _mapper.Map(request, post);

            if (await _postService.UpdatePostAsync(post))
                return Success($"Post with ID: {post.Id} updated successfully.");
            else
                return BadRequest<string>("Failed to update post.");
        }

        public async Task<Response<string>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);

            if (await _postService.DeletePostAsync(post))
                return Deleted<string>($"Post with ID : {post.Id} deleted successfully.");
            else
                return BadRequest<string>($"Failed to delete post with ID : {post.Id}.");
        }
    }
}
