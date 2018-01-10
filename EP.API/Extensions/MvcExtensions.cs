using EP.API.Filters;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EP.API.Extensions
{
    public static class MvcExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
                .AddMvcCore(opts =>
                {
                    opts.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddApiExplorer()
                .AddAuthorization(options =>
                {
                    options.AddPolicy("AdminAreas", policy =>
                    {
                        policy.AuthenticationSchemes.Add(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                        //policy.Requirements.Add(new MinimumAgeRequirement());
                    });
                })
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddJsonFormatters(settings => 
                {
                    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    settings.Formatting = Formatting.None;
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services;
        }
    }
}