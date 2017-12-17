using EP.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace EP.Data.AspNetIdentity
{
    public static class IdentityServiceCollectionExtensions
    {
		private const string AspNetUsersCollectionName = "AspNetUsers";
		private const string AspNetRolesCollectionName = "AspNetRoles";

        public static IdentityBuilder AddIdentityMongoStores(
            this IServiceCollection services,
            string connectionString)
        {
            return services.AddIdentityMongoStores<AppUser, AppRole>(connectionString);
        }

        public static IdentityBuilder AddIdentityMongoStores<TUser, TRole>(
            this IServiceCollection services,
            string connectionString) where TUser : AppUser where TRole : AppRole
		{
            IdentityBuilder builder = GetIdentityBuilder<TUser, TRole>(services);
            IMongoDatabase database = MongoDbHelper.GetMongoDatabase(connectionString);

            IMongoCollection<TUser> usersCollection = database.GetCollection<TUser>(AspNetUsersCollectionName);
			builder.Services.AddSingleton<IUserStore<TUser>>(p => new AppUserStore<TUser>(usersCollection));
			
			IMongoCollection<TRole> rolesCollection = database.GetCollection<TRole>(AspNetRolesCollectionName);
			builder.Services.AddSingleton<IRoleStore<TRole>>(p => new AppRoleStore<TRole>(rolesCollection));
			
			return builder;
		}

        private static IdentityBuilder GetIdentityBuilder<TUser, TRole>(IServiceCollection services)
            where TUser : AppUser where TRole : AppRole
        {
            return services.AddIdentity<TUser, TRole>(opts =>
            {
                // Password settings.
                PasswordOptions passwordOpts = opts.Password;
                passwordOpts.RequireDigit = false;
                passwordOpts.RequiredLength = 6;
                passwordOpts.RequireNonAlphanumeric = false;
                passwordOpts.RequireUppercase = false;
                passwordOpts.RequireLowercase = false;

                // Lockout settings.
                LockoutOptions lockoutOpts = opts.Lockout;
                lockoutOpts.AllowedForNewUsers = true;
                lockoutOpts.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                lockoutOpts.MaxFailedAccessAttempts = 5;

                // Cookie settings.
                //var cookieOpts = opts.Cookies.ApplicationCookie;
                //cookie.AccessDeniedPath = "";
                //cookie.AuthenticationScheme = "osCookie";
                //cookie.CookieName = ".osIdentity";
                //cookie.CookiePath = "/";
                //cookie.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("C:\\Github\\Identity\\artifacts"));
                //cookieOpts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //cookieOpts.LoginPath = new PathString("/admin/Authentication/SignIn");
                //cookieOpts.LogoutPath = new PathString("/admin/Authentication/SignOut");
                //cookieOpts.AccessDeniedPath = new PathString("/admin/Error/Forbidden");

                // User settings.
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                opts.User.RequireUniqueEmail = true;

                // SignIn settings.
                SignInOptions signinOpts = opts.SignIn;
                signinOpts.RequireConfirmedEmail = true;
                signinOpts.RequireConfirmedPhoneNumber = false;
            });
        }
    }
}