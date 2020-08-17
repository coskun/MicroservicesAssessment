using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Monitor.API.Controllers;

using Moq;

using RabbitMQ.Client;

using RabbitMqConnectionFactory;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace Monitor.UnitTests
{
    public class MonitorWebApiTest
    {
        private readonly Mock<ILogger<MonitorController>> _loggerMock;
        private readonly Mock<IRabbitMqConnectionFactory> _mockConnectionFactory;
        private readonly Mock<IModel> _mockRabbitMqModel;

        private MonitorController _mockMonitorController;

        public MonitorWebApiTest()
        {
            _loggerMock = new Mock<ILogger<MonitorController>>();
            _mockConnectionFactory = new Mock<IRabbitMqConnectionFactory>();
            _mockRabbitMqModel = new Mock<IModel>();
        }

        [Fact]
        public async Task Valid_Data_Should_Return_OK_Status_Code()
        {
            var postData = new API.Model.BindingModels.EventData[]
            {
                new API.Model.BindingModels.EventData
                {
                    App = Guid.NewGuid(),
                    Type = "HOTEL_SEARCH",
                    ActionDate = DateTime.Now,
                    IsSucceeded = true,
                    UserData = new API.Model.BindingModels.UserEventData
                    {
                        IsAuthenticated = true,
                        Provider = "b2c-internal",
                        Id = 12345,
                        EmailAddress = "coskun.bagriacik@gmail.com"
                    },
                    Attributes = new Dictionary<string, object>
                    {
                        { "HotelId", 125 },
                        { "HotelRegion", "Antalya" },
                        { "HotelName", "Rixos" }
                    }
                }
            };

            _mockConnectionFactory.Setup(x => x.GetModel()).Returns(_mockRabbitMqModel.Object);
            _mockMonitorController = new MonitorController(_loggerMock.Object, _mockConnectionFactory.Object);

            var result = await _mockMonitorController.PostEventAsync(postData);
            Assert.Equal((result as OkResult).StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Invalid_Data_Should_Return_BadRequest_Status_Code()
        {
            var postData = new API.Model.BindingModels.EventData[]
            {
                new API.Model.BindingModels.EventData
                {
                    App = Guid.NewGuid(),
                    ActionDate = DateTime.MinValue,
                    IsSucceeded = true,
                    UserData = null,
                    Attributes = null
                }
            };

            _mockConnectionFactory.Setup(x => x.GetModel()).Returns(_mockRabbitMqModel.Object);
            _mockMonitorController = new MonitorController(_loggerMock.Object, _mockConnectionFactory.Object);

            _mockMonitorController.ModelState.AddModelError("Type", "Type property is required");
            var result = await _mockMonitorController.PostEventAsync(postData);

            Assert.Equal((result as BadRequestResult).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Null_RabbitMq_Connection_Should_Return_InternalServerError_Status_Code()
        {
            var postData = new API.Model.BindingModels.EventData[]
            {
                new API.Model.BindingModels.EventData
                {
                    App = Guid.NewGuid(),
                    ActionDate = DateTime.MinValue,
                    IsSucceeded = true,
                    UserData = null,
                    Attributes = null
                }
            };

            _mockMonitorController = new MonitorController(_loggerMock.Object, _mockConnectionFactory.Object);
            var result = await _mockMonitorController.PostEventAsync(postData);

            Assert.Equal((result as StatusCodeResult).StatusCode, (int)HttpStatusCode.InternalServerError);
        }
    }
}