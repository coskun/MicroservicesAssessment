using EventBus.RabbitMQ.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Options;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Monitor.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddRazorPages();

            #region RabbitMq

            RabbitMqOptions rabbitMqOptions = new RabbitMqOptions
            {
                BrokerName = "test_broker",
                AutofacScopeName = "test_autofac",
                QueueName = "MainQueue",
                RetryCount = "5",
                Host = !string.IsNullOrEmpty(Configuration["RABBITMQ-HOST"]) ? Configuration["RABBITMQ-HOST"] : "docker.for.win.localhost",
                Username = !string.IsNullOrEmpty(Configuration["RABBITMQ-USER"]) ? Configuration["RABBITMQ-USER"] : "guest",
                Password = !string.IsNullOrEmpty(Configuration["RABBITMQ-PASS"]) ? Configuration["RABBITMQ-PASS"] : "guest",
                DispatchConsumersAsync = true
            };

            services.AddRabbitMqConnection(rabbitMqOptions);
            services.AddRabbitMqRegistration(rabbitMqOptions);

            #endregion

            #region API Documentation

            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Microservice Assessment - Monitor HTTP API",
                        Version = "v1",
                        Description = "Microservice Assessment - Monitor HTTP API"
                    });
                })
                .AddSwaggerGenNewtonsoftSupport();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appBasePath = Configuration["APP_BASE_PATH"];
            if (!string.IsNullOrEmpty(appBasePath))
                app.UsePathBase(appBasePath);

            #region API Documentation

            app.UseSwagger()
               .UseSwaggerUI(setup =>
               {
                   setup.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(appBasePath) ? appBasePath : string.Empty) }/swagger/v1/swagger.json", "Monitor.API V1");
               });

            #endregion

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}