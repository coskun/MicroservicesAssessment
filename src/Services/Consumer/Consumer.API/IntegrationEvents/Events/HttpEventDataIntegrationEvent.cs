using Consumer.API.Model;

using EventBus.Base.Standard;

namespace Consumer.API.IntegrationEvents.Events
{
    public class HttpEventDataIntegrationEvent : IntegrationEvent
    {
        public EventDataDTO[] EventData { get; set; }

        public HttpEventDataIntegrationEvent(EventDataDTO[] eventData)
        {
            EventData = eventData;
        }
    }
}