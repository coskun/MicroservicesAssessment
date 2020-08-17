using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Monitor.API.Model.BindingModels;

using Newtonsoft.Json;

using RabbitMQ.Client;

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
        private readonly IConfiguration _configuration;

        public MonitorController(ILogger<MonitorController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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
                // TODO : Move connection logic for persistent and shared connection

                var connFactory = new ConnectionFactory()
                {
                    HostName = !string.IsNullOrEmpty(_configuration["RABBITMQ-HOST"]) ? _configuration["RABBITMQ-HOST"] : "docker.for.win.localhost",
                    UserName = !string.IsNullOrEmpty(_configuration["RABBITMQ-USER"]) ? _configuration["RABBITMQ-USER"] : "guest",
                    Password = !string.IsNullOrEmpty(_configuration["RABBITMQ-PASS"]) ? _configuration["RABBITMQ-PASS"] : "guest",
                };

                using (var conn = connFactory.CreateConnection())
                using (var channel = conn.CreateModel())
                {
                    // TODO : Use EventBus with Integration events

                    channel.QueueDeclare("EventDataQueue", false, false, false, null);
                    channel.BasicPublish(string.Empty, routingKey: "EventDataQueue", basicProperties: null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventDataBindingModel)));
                }

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