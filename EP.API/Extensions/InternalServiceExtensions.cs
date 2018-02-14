using EP.API.Infrastructure;
using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Data.IdentityServerStore;
using EP.Services.Accounts;
using EP.Services.Blobs;
using EP.Services.Caching;
using EP.Services.Emails;
using EP.Services.Logs;
using EP.Services.Models;
using EP.Services.News;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LightInject.Microsoft.DependencyInjection;
using LightInject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace EP.API.Extensions
{
    public static class InternalServiceCollectionExtensions
    {
        public static IServiceProvider AddInternalServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionString)
        {
            services
                .Configure<AppSettings>(configuration.GetSection("AppSettings"))
                .AddSingleton<IAuthorizationHandler, FunctionPermissionHandler>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
                // AspNet Identity.
                .Register<IUserStore<AppUser>>(factory => GetUserCollection(connectionString), new PerContainerLifetime())
                .Register<IRoleStore<AppRole>>(factory => GetRoleCollection(connectionString), new PerContainerLifetime())
                // IdentityServer4.
                .Register<IResourceStore, MongoDbResourceStore>(new PerContainerLifetime())
                .Register<IClientStore, MongoDbClientStore>(new PerContainerLifetime())
                .Register<IPersistedGrantStore, MongoDbPersistedGrantStore>(new PerContainerLifetime());

            return container.CreateServiceProvider(services);
        }

        private static IUserStore<AppUser> GetUserCollection(
            string connectionString,
            string userCollectionName = "AspNetUsers")
        {
            var database = MongoDbHelper.GetMongoDatabase(connectionString);
            var userCollection = database.GetCollection<AppUser>(userCollectionName);
            IndexChecker.EnsureAppUserIndexes(userCollection).GetAwaiter().GetResult();

            return new AppUserStore<AppUser>(userCollection);
        }

        private static IRoleStore<AppRole> GetRoleCollection(
            string connectionString,
            string roleCollectionName = "AspNetRoles")
        {
            var database = MongoDbHelper.GetMongoDatabase(connectionString);
            var roleCollection = database.GetCollection<AppRole>(roleCollectionName);
            IndexChecker.EnsureAppRoleIndex(roleCollection).GetAwaiter().GetResult();

            return new AppRoleStore<AppRole>(roleCollection);
        }
    }
}
