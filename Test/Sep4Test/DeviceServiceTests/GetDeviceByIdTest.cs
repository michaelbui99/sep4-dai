using System;
using Domain;
using Moq;
using NUnit.Framework;
using WebAPI.Repositories;
using WebAPI.Services;

namespace Sep4Test.DeviceServiceTests
{
    [TestFixture]
    public class GetDeviceByIdTest
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
        public void GetDeviceById_DeviceDoesNotExist_Throws()
        {
            string deviceId = "bob1";

            ClimateDevice device = null;
            
            _deviceRepository.Setup<ClimateDevice>(x =>
                x.GetDeviceById(deviceId).Result).Returns(device);
            
            Assert.ThrowsAsync<ArgumentException>(async ()=> await _deviceService.GetDeviceById(deviceId));
        }
        
        [Test]
        public void GetDeviceById_DeviceExists_DoesNotThrows()
        {
            ClimateDevice device = new()
            {
                ClimateDeviceId = "bob1"
            };
            
            _deviceRepository.Setup<ClimateDevice>(x =>
                x.GetDeviceById(device.ClimateDeviceId).Result).Returns(device);
            
            Assert.DoesNotThrowAsync(async () => await _deviceService.GetDeviceById(device.ClimateDeviceId));

        }
        
        [TestCase(null)]
        [TestCase("")]
        public void GetDeviceById_InvalidArgument_Throws(string argument)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _deviceService.GetDeviceById(argument));
        }

    }
}