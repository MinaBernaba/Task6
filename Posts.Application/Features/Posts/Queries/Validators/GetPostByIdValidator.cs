using FluentValidation;
using Posts.Application.Features.Posts.Queries.Models;
using PostsProject.Infrastructure.Interfaces;

namespace Posts.Application.Features.Posts.Queries.Validators
{
    public class GetPostByIdValidator : AbstractValidator<GetPostByIdQuery>
    {
        private readonly IPostService _postService;

        public GetPostByIdValidator(IPostService postRepository)
        {
            _postService = postRepository;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post ID must be greater than 0.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PostId)
                    .MustAsync(async (postId, cancellation) => await _postService.IsExistAsync(p => p.Id == postId))
                    .WithMessage(p => $"Post with ID: {p.PostId} does not exist.")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.PostId)
                        .MustAsync(async (postId, cancellation) =>
                        {
                            var post = await _postService.GetByIdAsync(postId);
                            return post != null && post.IsPublic;
                        })
                        .WithMessage("The requested post is not public.");
                    });
                });
        }
    }

}
