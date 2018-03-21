using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EP.API.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, string policyName)
        {
            var corsBuilder = new CorsPolicyBuilder()
                .AllowAnyOrigin()
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
                
            return services.AddCors(opts =>
            {
                 opts.AddPolicy(policyName, corsBuilder.Build());
            });
        }
    }
}