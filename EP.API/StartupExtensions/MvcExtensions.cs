using EP.API.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace EP.API.StartupExtensions
{
    public static class MvcExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
                .AddMvc(opts =>
                {
                    opts.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddJsonOptions(opts =>
                {
                    var serializerSettings = opts.SerializerSettings;

                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializerSettings.Formatting = Formatting.None;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services;
        }
    }
}