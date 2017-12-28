using EP.API.StartupExtensions;
using EP.Data.AspNetIdentity;
using EP.Data.DbContext;
using EP.Data.Store;
using EP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

            Log.Logger = LogCreator.Create(_connectionString);

            ObjectMapper.RegisterMapping();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomCompression()
                .AddCustomCors("AllowAllOrigins")
                .AddMongoDbContext(_connectionString);

            services
                .AddCustomIdentity()
                .AddIdentityMongoStores(_connectionString)
                .AddDefaultTokenProviders();

            services
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .AddCustomMvc()
                .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
                .AddSingleton(Log.Logger)
                .AddCustomIdentityServer(_connectionString)
                .AddCustomSwaggerGen();

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
                //.UseAuthentication()
                .UseIdentityServer()
                .UseMongoDbForIdentityServer()
                .UseCustomSwagger()
                .UseMvcWithDefaultRoute()
                .InitDefaultData();
        }
    }
}
