using System;
using Castle.Components.DictionaryAdapter.Xml;
using Domain;
using Domain.Exceptions;
using Moq;
using NUnit.Framework;
using WebAPI.Repositories;
using WebAPI.Services;

namespace Sep4Test.DeviceServiceTests
{
    [TestFixture]
    public class AddNewDeviceTest
    {
        private IDeviceService _deviceService;
        private Mock<IDeviceRepository> _deviceRepository;

        [SetUp]
        public void SetUp()
        {
            _deviceRepository = new Mock<IDeviceRepository>();
            _deviceService = new DeviceService(_deviceRepository.Object);
        }


        [Test]
        public void AddNewDevice_IdDoesNotExist_DoesNotThrow()
        {
            ClimateDevice climateDevice = new ClimateDevice()
            {
                Actuators = null,
                ClimateDeviceId = "deviceID",
                Measurements = null,
                Sensors = null,
                Settings = null
            };
            ClimateDevice deviceNull = null;

            _deviceRepository.Setup<ClimateDevice>(x =>
                x.GetDeviceById(climateDevice.ClimateDeviceId).Result).Returns(deviceNull);
           // _deviceService.AddNewDevice(climateDevice);
          
            Assert.DoesNotThrowAsync(async () => await _deviceService.AddNewDevice(climateDevice));
            _deviceRepository.Verify(service => service.AddNewDevice(It.IsAny<ClimateDevice>()), Times.Once);
        }

        [Test]
        public void AddNewDevice_NullArgument_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _deviceService.AddNewDevice(null));
        }


        [Test]
        public void AddNewDevice_IdAlreadyExists_Throws()
        {
            ClimateDevice climateDevice = new ClimateDevice()
            {
                Actuators = null,
                ClimateDeviceId = "bob1",
                Measurements = null,
                Sensors = null,
                Settings = null
            };

            _deviceRepository.Setup<ClimateDevice>(x =>
                x.GetDeviceById(climateDevice.ClimateDeviceId).Result).Returns(climateDevice);


            Assert.ThrowsAsync<DeviceAlreadyExistsException>(async () =>
                await _deviceService.AddNewDevice(climateDevice));
        }
    }
}