using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace EP.API
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
                    Title = "EP API",
                    Description = "EP API",
                    TermsOfService = "None"/*,
                    Contact = new Contact
                    {
                        Name = "Talking Dotnet",
                        Email = "contact@talkingdotnet.com",
                        Url = "www.talkingdotnet.com"
                    }*/
                });
            });
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            return app.UseSwagger()
                .UseSwaggerUI(opts =>
                {
                    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "EP API V1");
                });
        }
    }
}