﻿using EP.API.Filters;
using EP.API.Infrastructure.DbContext;
using EP.API.Infrastructure.Logger;
using EP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDbContext<MongoDbContext>(opts =>
            {
                opts.ConnectionString = _connectionString;
                // Further configuration can be used as such:
                //opts.ClientSettings = new MongoClientSettings
                //{
                //    UseSsl = true,
                //    WriteConcern = WriteConcern.WMajority,
                //    ConnectionMode = ConnectionMode.Standalone
                //};
                //opts.DatabaseSettings = new MongoDatabaseSettings
                //{
                //    ReadPreference = ReadPreference.PrimaryPreferred,
                //    WriteConcern = new WriteConcern(1)
                //};
            });

            services
                //.AddCustomIdentity(_connectionString)
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
            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAllOrigins", corsBuilder.Build());
            });

            services
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
                .UseCors("AllowAllOrigins")
                //.UseAuthentication()
                .UseMvcWithDefaultRoute()
                .InitDefaultData();
        }
    }
}
