using Consumer.API.IntegrationEvents.Events;

using EventBus.Base.Standard;

using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

namespace Consumer.API.IntegrationEvents.EventHandlers
{
    public class HttpEventDataIntegrationEventHandler : IIntegrationEventHandler<HttpEventDataIntegrationEvent>
    {
        public HttpEventDataIntegrationEventHandler()
        {

        }

        public async Task Handle(HttpEventDataIntegrationEvent @event)
        {
            Console.WriteLine($"----- Incoming integration event: {@event.Id} to {Program.AppName} - ({@event})");
            Console.WriteLine($"----- EventData of {@event.Id} : {JsonConvert.SerializeObject(@event.EventData)}");
        }
    }
}