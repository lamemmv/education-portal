using EP.Data.IdentityServerStore;
using EP.Services;
using EP.Services.Accounts;
using EP.Services.Blobs;
using EP.Services.Caching;
using EP.Services.Emails;
using EP.Services.Logs;
using EP.Services.News;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace EP.API.Extensions
{
    public static class InternalServiceCollectionExtensions
    {
        public static IServiceProvider AddInternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            var container = new ServiceContainer(new ContainerOptions
            {
                EnablePropertyInjection = false
            });

            container
                .SetDefaultLifetime<PerScopeLifetime>()
                // Infrastructure.
                .Register<IMemoryCacheService, MemoryCacheService>()
                // IdentityServer4.
                .Register<ICorsPolicyService, InMemoryCorsPolicyService>()
                .Register<IProfileService, ProfileService>()
                // Emails.
                .Register<IEmailSender, NetEmailSender>()
                .Register<IEmailAccountService, EmailAccountService>()
                .Register<IQueuedEmailService, QueuedEmailService>()
                // Logs.
                .Register<IActivityLogService, ActivityLogService>()
                .Register<ILogService, LogService>()
                //Blobs.
                .Register<IBlobService, BlobService>()
                // News.
                .Register<INewsService, NewsService>();

            container
                // Background tasks.
                .Register<IHostedService, QueuedEmailSendTask>(new PerContainerLifetime())
                // AppSettings.
                .Register(factory => factory.GetInstance<IOptionsSnapshot<AppSettings>>().Value, new PerContainerLifetime())
                // IdentityServer4.
                .Register<IResourceStore, MongoDbResourceStore>(new PerContainerLifetime())
                .Register<IClientStore, MongoDbClientStore>(new PerContainerLifetime())
                .Register<IPersistedGrantStore, MongoDbPersistedGrantStore>(new PerContainerLifetime());

            return container.CreateServiceProvider(services);
        }
    }
}
