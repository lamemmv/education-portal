using EP.API.Extensions;
using EP.API.Filters;
using EP.API.Infrastructure;
using EP.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Serilog;
using System;

namespace EP.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly bool _isDevelopment;
        private readonly string _webRootPath;
        private readonly string _contentRootPath;
        private readonly AppSettings _appSettings;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _isDevelopment = env.IsDevelopment();
            _webRootPath = env.WebRootPath;
            _contentRootPath = env.ContentRootPath;

            _appSettings = new AppSettings();
            _configuration.Bind("AppSettings", _appSettings);

            Log.Logger = LogCreator.Create(_connectionString);
            ObjectMapper.RegisterMapping();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomCompression()
                .AddCustomCors("AllowAllOrigins")
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
                .AddSingleton(Log.Logger)
                .AddMongoDbContext(_connectionString);

            services
                .AddCustomIdentity(_appSettings.PasswordPolicies, _appSettings.LockoutPolicies, _appSettings.SignInPolicies)
                .AddCustomIdentityServer(_appSettings.HostUrl, _isDevelopment)
                .AddMvcCore(opts =>
                {
                    opts.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddJsonFormatters(settings =>
                {
                    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    settings.Formatting = Formatting.None;
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services
                .AddCustomSwaggerGen()
                .AddInternalServices(_configuration, _connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseResponseCompression()
                .UseCustomStaticFiles(_webRootPath, _contentRootPath, _appSettings.BlobFolders)
                .UseCors("AllowAllOrigins")
                .UseIdentityServer()
                .UseCustomSwagger()
                .UseMvcWithDefaultRoute()
                .InitDefaultData();
        }
    }
}
