using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Services.Accounts;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EP.API.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddCustomIdentityServer(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddCustomIdentity(connectionString);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<AppUser>()
                .AddProfileService<ProfileService>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(opts =>
                {
                    opts.Authority = "http://localhost:5000";
                    opts.RequireHttpsMetadata = false;
                    opts.ApiName = "ep.api";
                });

            return services;
        }

        private static IdentityBuilder AddCustomIdentity(
            this IServiceCollection services,
            string connectionString,
            string userCollectionName = "AspNetUsers",
            string roleCollectionName = "AspNetRoles")
        {
            var identityBuilder = services
                .AddIdentity<AppUser, AppRole>()
                .AddIdentityMongoDbStores(connectionString, userCollectionName, roleCollectionName)
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                // Password settings.
                PasswordOptions passwordOpts = opts.Password;
                passwordOpts.RequireDigit = false;
                passwordOpts.RequiredLength = 6;
                passwordOpts.RequireNonAlphanumeric = false;
                passwordOpts.RequireUppercase = false;
                passwordOpts.RequireLowercase = false;
                passwordOpts.RequiredUniqueChars = 0;

                // Lockout settings.
                LockoutOptions lockoutOpts = opts.Lockout;
                lockoutOpts.AllowedForNewUsers = true;
                lockoutOpts.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                lockoutOpts.MaxFailedAccessAttempts = 5;

                // User settings.
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                opts.User.RequireUniqueEmail = true;

                // SignIn settings.
                SignInOptions signinOpts = opts.SignIn;
                signinOpts.RequireConfirmedEmail = true;
                signinOpts.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings.
                // options.LoginPath = string.Empty;
                // options.LogoutPath = string.Empty;
                // options.AccessDeniedPath = string.Empty;
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return identityBuilder;
        }

        private static IdentityBuilder AddIdentityMongoDbStores(
            this IdentityBuilder identityBuilder,
            string connectionString,
            string userCollectionName,
            string roleCollectionName)
        {
            var services = identityBuilder.Services;
            var database = MongoDbHelper.GetMongoDatabase(connectionString);

            services.AddSingleton<IUserStore<AppUser>>(p =>
            {
                var userCollection = database.GetCollection<AppUser>(userCollectionName);
                IndexChecker.EnsureAppUserIndexes(userCollection).GetAwaiter().GetResult();

                return new AppUserStore<AppUser>(userCollection);
            });

            services.AddSingleton<IRoleStore<AppRole>>(p =>
            {
                var roleCollection = database.GetCollection<AppRole>(roleCollectionName);
                IndexChecker.EnsureAppRoleIndex(roleCollection).GetAwaiter().GetResult();

                return new AppRoleStore<AppRole>(roleCollection);
            });

            return identityBuilder;
        }
    }
}
