using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EP.API.StartupExtensions
{
    public static class StartupSwagger
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            return services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "EP API"
                });
            });
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(opts =>
                {
                    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "EP API v1");
                });
        }
    }
}