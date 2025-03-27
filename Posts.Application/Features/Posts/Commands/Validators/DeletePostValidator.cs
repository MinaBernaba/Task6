using FluentValidation;
using Microsoft.AspNetCore.Http;
using Posts.Application.Features.Posts.Commands.Models;
using PostsProject.Infrastructure.Interfaces;
using System.Security.Claims;

namespace Posts.Application.Features.Posts.Commands.Validators
{
    public class DeletePostValidator : AbstractValidator<DeletePostCommand>
    {
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeletePostValidator(IPostService postRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postRepository;
            _httpContextAccessor = httpContextAccessor;

            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post ID must be greater than 0.");
        }

        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.PostId)
                .MustAsync(async (postId, cancellation) => await _postService.IsExistAsync(p => p.Id == postId))
                .WithMessage(p => $"Post with ID: {p.PostId} does not exist.");

            RuleFor(x => x.PostId)
                .MustAsync(async (postId, cancellation) =>
                {
                    var userIdClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);

                    int userId = int.Parse(userIdClaim!.Value);
                    var post = await _postService.GetByIdAsync(postId);

                    return post != null && post.UserId == userId;
                })
                .WithMessage("You are not authorized to delete this post.");
        }
    }

}
