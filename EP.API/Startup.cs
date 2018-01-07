using EP.API.Extensions;
using EP.API.Infrastructure;
using EP.Data;
using EP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _hostingEnvironment = env;

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
                .AddCustomMvc()
                .AddCustomIdentityServer(_hostingEnvironment.IsDevelopment())
                .AddCustomSwaggerGen();

            return services.AddInternalServices(_configuration, _connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, AppSettings appSettings)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseResponseCompression()
                .UseCustomStaticFiles(
                    _hostingEnvironment.WebRootPath,
                    appSettings.PublicBlob,
                    appSettings.PrivateBlob)
                .UseCors("AllowAllOrigins")
                //.UseIdentityServer()
                .UseCustomSwagger()
                .UseMvcWithDefaultRoute()
                .InitDefaultData();
        }
    }
}
