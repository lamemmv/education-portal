using EP.Data.AspNetIdentity;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System;

namespace EP.API.StartupExtensions
{
    public static class StartupIdentity
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
                options.LoginPath = string.Empty;
                options.LogoutPath = string.Empty;
                options.AccessDeniedPath = string.Empty;
            });

            return builder;
        }

        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(GetApiResources())
                .AddInMemoryClients(GetClients())
                .AddTestUsers(GetUsers().ToList());

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "ep.api";
                });

            return services;
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("ep.api", "EP API");
        }

        private static IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                ClientId = "ep.web",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                ClientSecrets =
                {
                    new Secret("ep.web@P@SSW0RD".Sha256())
                },
                AllowedScopes =
                {
                    "ep.api"
                }
            };
        }

        private static IEnumerable<TestUser> GetUsers()
        {
            // yield return new TestUser
            // {
            //     SubjectId = "1",
            //     Username = "alice",
            //     Password = "password"
            // };
            yield return new TestUser
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "scott",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            };
        }
    }
}
