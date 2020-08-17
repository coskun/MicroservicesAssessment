using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Monitor.API.Model;

using Newtonsoft.Json;

using System;
using System.Net;
using System.Threading.Tasks;

namespace Monitor.API.Controllers
{
    [Route("api/v1/[controller]"), ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly ILogger<MonitorController> _logger;

        public MonitorController(ILogger<MonitorController> logger)
        {
            _logger = logger;
        }

        [Route("Event")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> PostEventAsync(EventData[] eventDataBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(); // 400

            try
            {
                // TODO : Publish

                _logger.LogTrace("Event consumed. EventData : {eventData}", JsonConvert.SerializeObject(eventDataBindingModel));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostEventAsync exception occured.");
                return StatusCode(500); // Internal Server Error
            }

            return Ok(); // 200 OK
        }
    }
}