﻿using EP.Services.Blobs;
using EP.Services.News;
using EP.Services;
using LightInject.Microsoft.DependencyInjection;
using LightInject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EP.API
{
    public static class StartupServices
    {
        public static IServiceProvider AddInternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            // If you need access to generic IConfiguration this is required.
            services.AddSingleton(x => configuration);

            // Add functionality to inject IOptions<T>.
            services.AddOptions();

            services.Configure<AppSettings>(appSettings =>
            {
                appSettings.ServerUploadFolder = configuration["AppSettings:ServerUploadFolder"];
            });

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
                //Blobs
                .Register<IBlobService, BlobService>()
                // News
                .Register<INewsService, NewsService>();

            return container.CreateServiceProvider(services);
        }
    }
}
