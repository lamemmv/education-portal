using EP.Data.AspNetIdentity;
using EP.Data.Store;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;

namespace EP.API.StartupExtensions
{
    public static class IdentityExtensions
    {
        public static IdentityBuilder AddCustomIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<AppUser, AppRole>(opts =>
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

            return builder;
        }

        public static IServiceCollection AddCustomIdentityServer(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(GetApiResources())
                //.AddInMemoryClients(GetClients())
                .AddMongoDbClients(connectionString)
                .AddTestUsers(GetUsers());
            //.AddAspNetIdentity<AppUser>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(opts =>
                {
                    opts.Authority = "http://localhost:5000";
                    opts.RequireHttpsMetadata = false;
                    opts.ApiName = "ep.api";
                });

            return services;
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("ep.api", "EP API");
        }

        // private static IEnumerable<Client> GetClients()
        // {
        //     yield return new Client
        //     {
        //         ClientId = "ep.web",
        //         AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
        //         ClientSecrets =
        //         {
        //             new Secret("ep.web@P@SSW0RD".Sha256())
        //         },
        //         AllowedScopes =
        //         {
        //             "ep.api"
        //         }
        //     };
        // }

        private static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "bob",
                    Password = "password"/*,
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "bob@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }*/
                }
            };
        }
    }
}
