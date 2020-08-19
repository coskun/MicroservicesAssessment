using EventBus.Base.Standard;

using Monitor.API.Model;

namespace Monitor.API.IntegrationEvents.Events
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