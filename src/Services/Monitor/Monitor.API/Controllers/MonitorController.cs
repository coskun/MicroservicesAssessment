using EventBus.Base.Standard;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Monitor.API.IntegrationEvents.Events;
using Monitor.API.Model;
using Monitor.API.Model.BindingModels;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Monitor.API.Controllers
{
    [Route("api/v1/[controller]"), ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly ILogger<MonitorController> _logger;
        private readonly IEventBus _eventBus;

        public MonitorController(ILogger<MonitorController> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [Route("Event")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> PostEventAsync(EventData[] eventDataBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                HttpEventDataIntegrationEvent @event = new HttpEventDataIntegrationEvent(eventDataBindingModel.Select(x => new EventDataDTO
                {
                    App = x.App,
                    Type = x.Type,
                    ActionDate = x.ActionDate,
                    IsSucceeded = x.IsSucceeded,
                    Metadata = x.Metadata,
                    Attributes = x.Attributes,
                    UserData = new UserEventDataDTO
                    {
                        IsAuthenticated = x.UserData.IsAuthenticated,
                        Provider = x.UserData.Provider,
                        Id = x.UserData.Id,
                        EmailAddress = x.UserData.EmailAddress
                    }
                }).ToArray());

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
                _eventBus.Publish(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostEventAsync exception occured.");
                return StatusCode(500); // Internal Server Error
            }

            return Ok();
        }
    }
}