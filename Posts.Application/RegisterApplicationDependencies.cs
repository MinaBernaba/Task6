using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Application.ServiceInterfaces;
using Posts.Application.Services;
using PostsProject.Application.ServiceInterfaces;
using PostsProject.Application.Services;
using PostsProject.Core.Bahaviors;
using System.Reflection;

namespace PostsProject.Application
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Configration for Mediator
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // Configration for AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Configration for Fluent validation 
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Configration for Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPostService, PostService>();

            return services;
        }
    }
}
