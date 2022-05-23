using System;
using System.Collections.Generic;
using Domain;
using Moq;
using NUnit.Framework;
using WebAPI.Repositories;
using WebAPI.Services;

namespace Sep4Test.MeasurementServiceTests
{
    [TestFixture]
    public class AddMeasurementTest
    {
        private Mock<IDeviceService> _deviceServiceMock;
        private Mock<IMeasurementRepository> _measurementRepositoryMock;
        private IMeasurementService _measurementService;

        [SetUp]
        public void SetUp()
        {
            _deviceServiceMock = new Mock<IDeviceService>();
            _measurementRepositoryMock = new Mock<IMeasurementRepository>();
            _measurementService = new MeasurementService(_measurementRepositoryMock.Object, _deviceServiceMock.Object);
        }

        [Test]
        public void AddMeasurement_DeviceDoesNotExist_CreatesNewDevice()
        {
            // Arrange
            var nonExistingDeviceId = "testDevice";
            var testMeasurements = new List<Measurement>()
            {
                new Measurement()
                {
                    Co2 = 200,
                    Temperature = 20,
                    Humidity = 10,
                    Timestamp = DateTime.Now
                }
            };

            _deviceServiceMock.Setup(service => service.GetDeviceById(nonExistingDeviceId)).Throws<ArgumentException>();

            // Act 
            _measurementService.AddMeasurements(nonExistingDeviceId, testMeasurements);

            // Assert
            _deviceServiceMock.Verify(service => service.GetDeviceById(nonExistingDeviceId), Times.Once);
            _deviceServiceMock.Verify(service =>
                    service.AddNewDevice(It.IsAny<ClimateDevice>()),
                Times.Once); // This is only called if a device does not exist
            _measurementRepositoryMock.Verify(service =>
                service.AddMeasurements(nonExistingDeviceId, testMeasurements), Times.Once);
        }

        [Test]
        public void AddMeasurement_DeviceDoesExist_DoesNotCreateNewDevice()
        {
            // Arrange
            var existingDevice = new ClimateDevice()
            {
                ClimateDeviceId = "testDevice",
                Measurements = new List<Measurement>(),
                Sensors = new List<Sensor>(),
                Actuators = new List<Actuator>()
            };

            var testMeasurements = new List<Measurement>()
            {
                new Measurement()
                {
                    Co2 = 200,
                    Temperature = 20,
                    Humidity = 10,
                    Timestamp = DateTime.Now
                }
            };

            _deviceServiceMock
                .Setup<ClimateDevice>(service => service.GetDeviceById(existingDevice.ClimateDeviceId).Result)
                .Returns(existingDevice);

            // Act 
            _measurementService.AddMeasurements(existingDevice.ClimateDeviceId, testMeasurements);

            // Assert
            _deviceServiceMock.Verify(service => service.GetDeviceById(existingDevice.ClimateDeviceId), Times.Once);
            _deviceServiceMock.Verify(service =>
                    service.AddNewDevice(It.IsAny<ClimateDevice>()),
                Times.Never); // This is only called if a device does not exist
            _measurementRepositoryMock.Verify(service =>
                service.AddMeasurements(existingDevice.ClimateDeviceId, testMeasurements), Times.Once);
        }
    }
}

