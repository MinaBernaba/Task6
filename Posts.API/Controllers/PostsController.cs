using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Posts.Application.Features.Posts.Commands.Models;
using Posts.Application.Features.Posts.Queries.Models;
using PostsProject.api.Base;

namespace Posts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController(IMediator _mediator) : AppControllerBase
    {
        [HttpGet("GetAllApprovedPosts")]
        public async Task<IActionResult> GetAllApprovedPosts()
            => NewResult(await _mediator.Send(new GetAllApprovedPostsQuery()));

        [HttpGet("GetAllPosts")]
        [Authorize(Roles = "Reviewer, Admin")]
        public async Task<IActionResult> GetAllPosts()
            => NewResult(await _mediator.Send(new GetAllPostsQuery()));


        [HttpGet("GetPostById/{postId}")]
        public async Task<IActionResult> GetAllPosts(int postId) =>
            NewResult(await _mediator.Send(new GetPostByIdQuery() { PostId = postId }));


        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(CreatePostCommand createPost)
            => NewResult(await _mediator.Send(createPost));

        [HttpPut("UpdatePost")]
        public async Task<IActionResult> UpdatePost(UpdatePostCommand updatePost)
           => NewResult(await _mediator.Send(updatePost));

        [HttpPatch("ApprovePost/{postId}")]
        [Authorize(Roles = "Reviewer")]
        public async Task<IActionResult> ApprovePost(int postId)
            => NewResult(await _mediator.Send(new ApprovePostCommand() { PostId = postId }));



        [HttpDelete("DeletePost/{postId}")]
        public async Task<IActionResult> DeletePost(int postId) =>
            NewResult(await _mediator.Send(new DeletePostCommand() { PostId = postId }));

    }
}
