﻿using EP.API.Filters;
using EP.API.Infrastructure.DbContext;
using EP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace EP.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            StartupMapper.RegisterMapping();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            services.AddMongoDbContext<MongoDbContext>(opts =>
            {
                opts.ConnectionString = _configuration.GetSection("MongoDb:ConnectionString").Value;
                opts.DatabaseName = _configuration.GetSection("MongoDb:Database").Value;
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
