using FluentValidation;
using Posts.Application.Features.Posts.Commands.Models;
using Posts.Application.ServiceInterfaces;

namespace Posts.Application.Features.Posts.Commands.Validators
{
    public class ApprovePostValidator : AbstractValidator<ApprovePostCommand>
    {
        private readonly IPostService postService;

        public ApprovePostValidator(IPostService _postService)
        {
            postService = _postService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        private void ApplyValidationRules()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");
        }
        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.PostId)
                .MustAsync(async (postId, cancellation) => await postService.IsPostExistByIdAsync(postId))
                .WithMessage(p => $"Post with ID: {p.PostId} does not exist.");
        }
    }
}
