using Consumer.API.Extensions;

using EventBus.Base.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Options;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json.Serialization;

namespace Consumer.API
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
            //services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory.RabbitMqConnectionFactory>();

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
            services.AddEventBusHandling(EventBusExtension.GetHandlers());

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.SubscribeToEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}