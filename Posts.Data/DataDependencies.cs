using PostsProject.Data.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PostsProject.Data
{
    public static class DataDependencies
    {
        public static IServiceCollection RegisterDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JWT"));
            return services;
        }
    }
}
