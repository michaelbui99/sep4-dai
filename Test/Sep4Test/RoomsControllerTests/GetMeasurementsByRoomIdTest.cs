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
    public class GetMeasurementsByRoomIdTest
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
        
        public async Task GetMeasurementByRoomId_ReturnsStatusCode500()
        {
            //Arrange
            int ? roomId = -1;
            var result = (await _roomsControllermock.GetMeasurementByRoomId(roomId.Value)).Result;
            //Assert 
            //if it's of type object result it means the http status error is 500
            Assert.IsInstanceOf<ObjectResult>(result);
        }
        
        [Test]
        
        public async Task GetMeasurementByRoomId_ReturnsNotFoundResult()
        {
            //Arrange
            int roomId = -2;
            _roomService.Setup(service => service.GetMeasurementsByRoomIdAsync(roomId)).Throws<ArgumentException>();
            
            //Act
            var result = (await _roomsControllermock.GetMeasurementByRoomId(roomId)).Result;
            //Assert 
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
        
        
        [Test]
        
        public async Task GetMeasurementByRoomId_ReturnsOkResult()
        {
            //Arrange
            int roomId = 2;
            
            //Act
            var result = (await _roomsControllermock.GetMeasurementByRoomId(roomId)).Result;
            //Assert 
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
    
}

