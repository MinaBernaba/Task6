using FluentValidation;
using MediatR;
using Posts.Application.Behaviors;
using System.Net;

namespace PostsProject.Core.Bahaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var errors = failures.Select(x => $"Error in {x.PropertyName}: {x.ErrorMessage}").ToList();

                    // Check for specific validation errors to determine the status code

                    if (failures.Any(f => f.ErrorMessage.Contains("not authorized")))
                        throw new CustomValidationException(HttpStatusCode.Forbidden, errors);

                    if (failures.Any(f => f.ErrorMessage.Contains("does not exist")))
                        throw new CustomValidationException(HttpStatusCode.NotFound, errors);

                    else
                        throw new CustomValidationException(HttpStatusCode.BadRequest, errors);

                }
            }
            return await next();
        }
    }

}
