using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebAPI.Controllers;
using WebAPI.Services;

namespace Sep4Test.RoomsControllerTests
{
    [TestFixture]
    public class GetMeasurementByRoomNameTest
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
        
        public async Task GetMeasurementByRoomName_ReturnsKeyNotFoundException()
        {
            //Arrange
            String roomName = "sally";
            long l1 = 125478963;
            long l2 = 1365897421;   
            _roomService.Setup(service => service.GetRoomMeasurementsBetweenAsync(l1, l2, roomName)).Throws<KeyNotFoundException>();
            
            //Act
            var result = (await _roomsControllermock.GetMeasurementByRoomName(roomName, l1, l2)).Result;
            //Assert 
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            
        }
        
        [Test]
        
        public async Task GetMeasurementByRoomName_ReturnsNotFound()
        {
          
            //Arrange
            String roomName = " ";
            _roomService.Setup(service => service.GetRoomByNameAsync(roomName)).Throws<ArgumentException>();
            
            //Act
            var result = (await _roomsControllermock.GetMeasurementByRoomName(roomName, null, null)).Result;
            //Assert 
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
        
     
        
        
        [Test]
        
        public async Task GetMeasurementByRoomName_ReturnsStatusCode500()
        {
            //Arrange
            String roomName = null;
            var result = (await _roomsControllermock.GetMeasurementByRoomName(roomName, 125478963,1365897421)).Result;
            //Assert 
            //if it's of type object result it means the http status error is 500
            Assert.IsInstanceOf<ObjectResult>(result);
        }
        
    }
}

