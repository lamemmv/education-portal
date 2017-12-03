using EP.Services.News;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EP.API
{
    public static class StartupServices
    {
        public static IServiceProvider AddInternalServices(this IServiceCollection services)
        {
            var container = new ServiceContainer(new ContainerOptions
            {
                EnablePropertyInjection = false
            });

            container
                .SetDefaultLifetime<PerScopeLifetime>()
                // Infrastructure.
                //.Register<IMemoryCacheService, MemoryCacheService>()
                // Logs.
                //.Register<IActivityLogTypeService, ActivityLogTypeService>()
                //.Register<IActivityLogService, ActivityLogService>()
                //.Register<ILogService, LogService>()
                // News
                .Register<INewsService, NewsService>();

            return container.CreateServiceProvider(services);
        }
    }
}
