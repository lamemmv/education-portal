using EP.API.Filters;
using EP.Data;
using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Data.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO.Compression;

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
            // Enable CORS.
            var corsBuilder = new CorsPolicyBuilder()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();
            services.AddCors(opts =>
            {
                 opts.AddPolicy("AllowAllOrigins", corsBuilder.Build());
            });

            services.AddMongoDbContext(_connectionString);

            services
                .AddIdentityMongoStores(_connectionString)
                .AddDefaultTokenProviders();

            services
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .AddMvc(opts =>
                {
                    opts.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddJsonOptions(opts =>
                {
                    var serializerSettings = opts.SerializerSettings;

                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializerSettings.Formatting = Formatting.None;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

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
                .UseCustomStaticFiles(
                    env.WebRootPath,
                    _configuration["AppSettings:PublicBlob"],
                    _configuration["AppSettings:PrivateBlob"])
                .UseCors("AllowAllOrigins")
                .UseCustomSwagger()
                .UseIdentityServer()
                .UseMvcWithDefaultRoute()
                .InitDefaultData();
        }

        private static IEnumerable<string> MimeTypes
        {
            get
            {
                // General.
                yield return "text/plain";
                // Static files.
                //yield return "text/css";
                //yield return "application/javascript";
                // MVC.
                //yield return "text/html";
                //yield return "application/xml";
                //yield return "text/xml";
                yield return "application/json";
                yield return "text/json";
                // Custom.
                yield return "image/svg+xml";
            }
        }
    }
}
