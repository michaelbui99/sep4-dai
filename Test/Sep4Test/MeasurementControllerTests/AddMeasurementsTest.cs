using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebAPI.Controllers;
using WebAPI.DTO;
using WebAPI.Repositories;
using WebAPI.Services;

namespace Sep4Test.MeasurementControllerTests
{
    [TestFixture]
    
    public class AddMeasurementsTest
    {
        private MeasurementsController _measurementsControllermock;
        private Mock<IMeasurementService> _measurementService;
        private Mock<ILogger<MeasurementsController>> _logger;
        
        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<MeasurementsController>>();
            _measurementService = new Mock<IMeasurementService>();
            _measurementsControllermock = new MeasurementsController(_measurementService.Object, _logger.Object);
        }

        [Test]

        public async Task CreateAddMeasurementsRequest_ReturnsStatusCode500()
        {
            //Arrange
            PostMeasurmentsDTO measurmentsDto = null;
            var result = (await _measurementsControllermock.AddMeasurements(measurmentsDto)).Result;
            //Assert 
            //if it's of type object result it means the http status error is 500
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]

        public async Task CreateAddMeasurementsRequest_ReturnsBadRequestResults()
        {
            //Arrange
            IList<Measurement> Measurements = new List<Measurement>();
            Measurements.Add(new Measurement());
            PostMeasurmentsDTO measurmentsDto = new PostMeasurmentsDTO();
            measurmentsDto.DeviceId = null;
            measurmentsDto.Measurements = Measurements;
            _measurementService.Setup(service => service.AddMeasurements(null, Measurements))
                .Throws<ArgumentException>();
            
            //Act
            var result = (await _measurementsControllermock.AddMeasurements(measurmentsDto)).Result;
            
            //Assert 
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
        [Test]

        public async Task CreateAddMeasurementsRequest_ReturnsOkResults()
        {
            //Arrange
            IList<Measurement> Measurements = new List<Measurement>();
            Measurements.Add(new Measurement());
            PostMeasurmentsDTO measurmentsDto = new PostMeasurmentsDTO();
            measurmentsDto.DeviceId = "one";
            measurmentsDto.Measurements = Measurements;
            
            //Act
            var result = (await _measurementsControllermock.AddMeasurements(measurmentsDto)).Result;
            
            //Assert 
            Assert.IsInstanceOf<OkResult>(result);
        }
        
        
    }
    

}

