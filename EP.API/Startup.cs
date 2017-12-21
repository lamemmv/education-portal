using EP.API.Filters;
using EP.API.Infrastructure.Logger;
using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System;

namespace EP.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.MongoDb(_connectionString, restrictedToMinimumLevel: LogEventLevel.Warning)
                .CreateLogger();

            StartupMapper.RegisterMapping();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDbContext(_connectionString);

            services
                .AddIdentityMongoStores(_connectionString)
                .AddDefaultTokenProviders();

            services
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .AddMvc(opts =>
                {
                    var filters = opts.Filters;

                    filters.Add(typeof(ValidateViewModelFilter));
                    filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddJsonOptions(opts =>
                {
                    var serializerSettings = opts.SerializerSettings;

                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializerSettings.Formatting = Formatting.None;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // Enable CORS.
            var corsBuilder = new CorsPolicyBuilder()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();

            services
                .AddResponseCompression(opts =>
                {
                    opts.EnableForHttps = true;
                    opts.Providers.Add<GzipCompressionProvider>();
                    opts.MimeTypes = MimeTypes;
                })
                .Configure<GzipCompressionProviderOptions>(opts =>
                {
                    opts.Level = CompressionLevel.Fastest;
                })
                .AddCors(opts =>
                {
                    opts.AddPolicy("AllowAllOrigins", corsBuilder.Build());
                })
                .AddCustomSwaggerGen()
                .AddCustomIdentity()
                .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
                .AddSingleton(Log.Logger);

            return services.AddInternalServices(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseResponseCompression()
                .UseCors("AllowAllOrigins")
                .UseCustomSwagger()
                .UseIdentityServer()
                .UseMvcWithDefaultRoute()
                .InitDefaultData();
        }

        private static readonly IEnumerable<string> MimeTypes = new[]
        {
            // General.
            "text/plain",
            // Static files.
            //"text/css",
            //"application/javascript",
            // MVC.
            //"text/html",
            //"application/xml",
            //"text/xml",
            "application/json",
            "text/json",
            // Custom.
            "image/svg+xml"
        };
    }
}
