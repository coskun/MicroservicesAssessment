using Consumer.API.IntegrationEvents.EventHandlers;
using Consumer.API.IntegrationEvents.Events;

using EventBus.Base.Standard;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

namespace Consumer.API.Extensions
{
    public static class EventBusExtension
    {
        public static IEnumerable<IIntegrationEventHandler> GetHandlers()
        {
            return new List<IIntegrationEventHandler>
            {
                new HttpEventDataIntegrationEventHandler()
            };
        }

        public static IApplicationBuilder SubscribeToEvents(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<HttpEventDataIntegrationEvent, HttpEventDataIntegrationEventHandler>();

            return app;
        }

    }
}