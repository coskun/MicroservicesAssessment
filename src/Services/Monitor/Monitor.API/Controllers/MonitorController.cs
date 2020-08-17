using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Monitor.API.Model.BindingModels;

using Newtonsoft.Json;

using RabbitMQ.Client;

using RabbitMqConnectionFactory;

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.API.Controllers
{
    [Route("api/v1/[controller]"), ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly ILogger<MonitorController> _logger;
        private readonly IRabbitMqConnectionFactory _connectionFactory;

        public MonitorController(ILogger<MonitorController> logger, IRabbitMqConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
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
                // TODO : Use EventBus with Integration events
                var channel = _connectionFactory.GetModel();

                channel.QueueDeclare("EventDataQueue", false, false, false, null);
                channel.BasicPublish(string.Empty, routingKey: "EventDataQueue", basicProperties: null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventDataBindingModel)));

                Console.WriteLine($"Event consumed. EventData : {JsonConvert.SerializeObject(eventDataBindingModel)}");
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