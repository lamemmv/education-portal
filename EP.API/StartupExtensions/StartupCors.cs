using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EP.API.StartupExtensions
{
    public static class StartupCors
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, string policyName)
        {
            var corsBuilder = new CorsPolicyBuilder()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();
                
            return services.AddCors(opts =>
            {
                 opts.AddPolicy(policyName, corsBuilder.Build());
            });
        }
    }
}