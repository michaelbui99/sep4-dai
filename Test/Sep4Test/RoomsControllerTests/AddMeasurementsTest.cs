using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebAPI.Controllers;
using WebAPI.DTO;
using WebAPI.Services;

namespace Sep4Test.RoomsControllerTests
{
    [TestFixture]
    public class AddMeasurementsTest
    {
        private RoomsController _roomsControllermock;
        private Mock<IRoomService> _roomService;
        private Mock<ILogger<RoomsController>> _logger;

        [SetUp]

        public void SetUp()
        {
            _logger = new Mock<ILogger<RoomsController>>();
            _roomService = new Mock<IRoomService>();
            _roomsControllermock = new RoomsController(_roomService.Object, _logger.Object);
        }
        
        
        [Test]
        
        public async Task AddMeasurements_ReturnsNotFoundResult()
        {
            //Arrange
            String roomName = "Living";
            PostMeasurmentsDTO measurmentsDto = new PostMeasurmentsDTO();
            measurmentsDto.DeviceId = "sally";
            _roomService.Setup(service => service.AddMeasurementsAsync(measurmentsDto.DeviceId,roomName,measurmentsDto.Measurements)).Throws<ArgumentException>();
            
            //Act
            var result = (await _roomsControllermock.AddMeasurements(roomName,measurmentsDto));
            //Assert 
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
        
        [Test]
        public async Task AddMeasurements_ReturnsStatusCode500()
        {
            //Arrange
            String roomName = null;
            PostMeasurmentsDTO measurmentsDto = null;
            var result = (await _roomsControllermock.AddMeasurements(roomName, measurmentsDto));
            //Assert 
            //if it's of type object result it means the http status error is 500
            Assert.IsInstanceOf<ObjectResult>(result);
        }
        
    }
}

