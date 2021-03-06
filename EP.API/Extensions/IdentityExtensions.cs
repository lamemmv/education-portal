﻿using EP.Data.AspNetIdentity;
using EP.Services.Accounts;
using EP.Services.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EP.API.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddCustomIdentity(
            this IServiceCollection services,
            PasswordOptions passwordPolicies,
            LockoutPolicies lockoutPolicies,
            SignInOptions signinPolicies)
        {
            services
                .AddIdentity<AppUser, AppRole>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                // Password settings.
                PasswordOptions passwordOpts = opts.Password;
                passwordOpts.RequireDigit = passwordPolicies.RequireDigit;
                passwordOpts.RequiredLength = passwordPolicies.RequiredLength;
                passwordOpts.RequiredUniqueChars = passwordPolicies.RequiredUniqueChars;               
                passwordOpts.RequireNonAlphanumeric = passwordPolicies.RequireNonAlphanumeric;
                passwordOpts.RequireLowercase = passwordPolicies.RequireLowercase;
                passwordOpts.RequireUppercase = passwordPolicies.RequireUppercase;

                // Lockout settings.
                LockoutOptions lockoutOpts = opts.Lockout;
                lockoutOpts.AllowedForNewUsers = lockoutPolicies.AllowedForNewUsers;
                lockoutOpts.MaxFailedAccessAttempts = lockoutPolicies.MaxFailedAccessAttempts;
                lockoutOpts.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutPolicies.DefaultLockoutInMinutes);

                // User settings.
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                opts.User.RequireUniqueEmail = true;

                // SignIn settings.
                SignInOptions signinOpts = opts.SignIn;
                signinOpts.RequireConfirmedEmail = signinPolicies.RequireConfirmedEmail;
                signinOpts.RequireConfirmedPhoneNumber = signinPolicies.RequireConfirmedPhoneNumber;
            });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings.
            //    // options.LoginPath = string.Empty;
            //    // options.LogoutPath = string.Empty;
            //    // options.AccessDeniedPath = string.Empty;
            //    options.Events = new CookieAuthenticationEvents
            //    {
            //        OnRedirectToLogin = ctx =>
            //        {
            //            if (ctx.Request.Path.StartsWithSegments("/api") &&
            //                ctx.Response.StatusCode == (int)HttpStatusCode.OK)
            //            {
            //                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //            }
            //            else
            //            {
            //                ctx.Response.Redirect(ctx.RedirectUri);
            //            }

            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            return services;
        }

        public static IServiceCollection AddCustomIdentityServer(
            this IServiceCollection services,
            string hostUrl,
            bool isDevelopment)
        {
            services
                .AddIdentityServer(opts =>
                {
                    var endpoints = opts.Endpoints;
                    endpoints.EnableAuthorizeEndpoint = false;
                    endpoints.EnableUserInfoEndpoint = false;
                    endpoints.EnableEndSessionEndpoint = false;
                    endpoints.EnableCheckSessionEndpoint = false;
                    endpoints.EnableIntrospectionEndpoint = false;

                    var discovery = opts.Discovery;
                    discovery.ShowEndpoints = false;
                    //discovery.ShowKeySet = false;
                    discovery.ShowIdentityScopes = false;
                    discovery.ShowApiScopes = false;
                    discovery.ShowClaims = false;
                    discovery.ShowResponseTypes = false;
                    discovery.ShowResponseModes = false;
                    discovery.ShowGrantTypes = false;
                    discovery.ShowExtensionGrantTypes = false;
                    discovery.ShowTokenEndpointAuthenticationMethods = false;
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<AppUser>()
                .AddProfileService<ProfileService>();

            services
                .AddAuthentication(opts =>
                {
                    //opts.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    opts.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    //opts.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    //opts.DefaultSignOutScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    //opts.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(opts =>
                {
                    opts.ApiName = "ep.api.admin";
                    opts.ApiSecret = "ep.api.admin@P@SSW0RD";
                    opts.Authority = hostUrl;
                    opts.RequireHttpsMetadata = !isDevelopment;
                });

            return services;
        }
    }
}
