using EP.Services;
using EP.Services.Blobs;
using EP.Services.Caching;
using EP.Services.Emails;
using EP.Services.Logs;
using EP.Services.News;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .Register<IMemoryCacheService, MemoryCacheService>()
                // Emails.
                .Register<IEmailSender, NetEmailSender>()
                .Register<IEmailAccountService, EmailAccountService>()
                .Register<IQueuedEmailService, QueuedEmailService>()
                // Logs.
                .Register<IActivityLogService, ActivityLogService>()
                //.Register<ILogService, LogService>()
                //Blobs.
                .Register<IBlobService, BlobService>()
                // News.
                .Register<INewsService, NewsService>();

            // Background tasks.
            container.Register<IHostedService, QueuedEmailSendTask>(new PerContainerLifetime());

            return container.CreateServiceProvider(services);
        }
    }
}
