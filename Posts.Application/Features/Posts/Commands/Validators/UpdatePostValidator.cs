using FluentValidation;
using Microsoft.AspNetCore.Http;
using Posts.Application.Features.Posts.Commands.Models;
using PostsProject.Infrastructure.Interfaces;
using System.Security.Claims;

namespace Posts.Application.Features.Posts.Commands.Validators
{
    public class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
    {
        private readonly IPostService _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdatePostValidator(IPostService postRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Post ID must be greater than 0.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {

                    RuleFor(x => x.PostId)
                       .MustAsync(async (postId, cancellation) => await _postRepository.IsExistAsync(p => p.Id == postId))
                       .WithMessage(p => $"Post with ID: {p.PostId} does not exist.")
                       .WithErrorCode("404")
                       .DependentRules(() =>
                       {
                           RuleFor(x => x.PostId)
                               .MustAsync(async (postId, cancellation) =>
                               {
                                   var userIdClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);

                                   int userId = int.Parse(userIdClaim!.Value);

                                   var post = await _postRepository.GetByIdAsync(postId);

                                   return post != null && post.UserId == userId;
                               })
                               .WithMessage("You are not authorized to update this post.")
                               .WithErrorCode("403");
                       });
                });

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .WithErrorCode("400")
                .MaximumLength(30).WithMessage("Title must not exceed 30 characters.")
                 .WithErrorCode("400");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body is required.")
                .WithErrorCode("400")
                .MaximumLength(2000).WithMessage("Body must not exceed 2000 characters.")
                .WithErrorCode("400");
        }

    }

}
