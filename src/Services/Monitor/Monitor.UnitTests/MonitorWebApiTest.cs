using EventBus.Base.Standard;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Monitor.API.Controllers;
using Monitor.API.IntegrationEvents.Events;

using Moq;

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
        private readonly Mock<IEventBus> _eventBusMock;

        private MonitorController _mockMonitorController;

        public MonitorWebApiTest()
        {
            _loggerMock = new Mock<ILogger<MonitorController>>();
            _eventBusMock = new Mock<IEventBus>();
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

            _mockMonitorController = new MonitorController(_loggerMock.Object, _eventBusMock.Object);
            var result = await _mockMonitorController.PostEventAsync(postData);

            _eventBusMock.Verify(mock => mock.Publish(It.IsAny<HttpEventDataIntegrationEvent>()), Times.Once);

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

            _mockMonitorController = new MonitorController(_loggerMock.Object, _eventBusMock.Object);

            _mockMonitorController.ModelState.AddModelError("Type", "Type property is required");
            var result = await _mockMonitorController.PostEventAsync(postData);

            Assert.Equal((result as BadRequestResult).StatusCode, (int)HttpStatusCode.BadRequest);
        }
    }
}