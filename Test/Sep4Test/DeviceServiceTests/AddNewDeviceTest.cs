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
        private Mock<IRoomService> _roomService;

        [SetUp]
        public void SetUp()
        {
            _deviceRepository = new Mock<IDeviceRepository>();
            _roomService = new Mock<IRoomService>();
            _deviceService = new DeviceService(_deviceRepository.Object, _roomService.Object);
        }


        [Test]
        public void AddNewDevice_IdDoesNotExist_DoesNotThrow()
        {
            ClimateDevice climateDevice = new ClimateDevice()
            {
                ClimateDeviceId = "deviceID",
                Measurements = null,
                Settings = null
            };
            ClimateDevice deviceNull = null;

            _deviceRepository.Setup<ClimateDevice>(x =>
                x.GetDeviceByIdAsync(climateDevice.ClimateDeviceId).Result).Returns(deviceNull);
           // _deviceService.AddNewDevice(climateDevice);
          
            Assert.DoesNotThrowAsync(async () => await _deviceService.AddNewDeviceAsync(climateDevice));
            _deviceRepository.Verify(service => service.AddNewDeviceAsync(It.IsAny<ClimateDevice>()), Times.Once);
        }

        [Test]
        public void AddNewDevice_NullArgument_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _deviceService.AddNewDeviceAsync(null));
        }


        [Test]
        public void AddNewDevice_IdAlreadyExists_Throws()
        {
            ClimateDevice climateDevice = new ClimateDevice()
            {
                ClimateDeviceId = "bob1",
                Measurements = null,
                Settings = null
            };

            _deviceRepository.Setup<ClimateDevice>(x =>
                x.GetDeviceByIdAsync(climateDevice.ClimateDeviceId).Result).Returns(climateDevice);


            Assert.ThrowsAsync<DeviceAlreadyExistsException>(async () =>
                await _deviceService.AddNewDeviceAsync(climateDevice));
        }
    }
}