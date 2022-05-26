using System;
using System.Collections.Generic;
using Domain;
using Moq;
using NUnit.Framework;
using WebAPI.Repositories;
using WebAPI.Services;

namespace Sep4Test.RoomServiceTests
{
    [TestFixture]
    public class SetSettingsToRoomTest
    {
        private IRoomService _roomService;
        private Mock<IRoomRepository> _roomRepository;
        private Mock<IMeasurementRepository> _measurementRepository;
        private Room _room;
        private IList<Measurement> _measurements;
        private IList<ClimateDevice> _climateDevices;
        private ClimateDevice _climateDevice;
        private Measurement _measurement;
        private Settings _settings;

        [SetUp]
        public void SetUp()
        {
            _settings = new Settings()
            {
                SettingId = 1,
                Co2Threshold = 1000,
                HumidityThreshold = 30,
                TargetTemperature = 25.1F,
                TemperatureMargin = 10
            };

            _measurement = new Measurement()
            {
                MeasurementId = 1,
                Co2 = 1000,
                Humidity = 40,
                Temperature = 20F,
                Timestamp = DateTime.Now
            };
            _measurements = new List<Measurement>();
            _measurements.Add(_measurement);

            _climateDevice = new ClimateDevice()
            {
                ClimateDeviceId = "Test1",
                Actuators = null,
                Measurements = _measurements,
                Settings = _settings
            };
            _climateDevices = new List<ClimateDevice>();
            _climateDevices.Add(_climateDevice);
            
            _room = new Room()
            {
                RoomId = 1,
                RoomName = "C02_02",
                ClimateDevices = _climateDevices,
                Settings = _settings
            };

            _roomRepository = new Mock<IRoomRepository>();
            _measurementRepository = new Mock<IMeasurementRepository>();
            _roomService = new RoomService(_roomRepository.Object, _measurementRepository.Object);
        }

        [Test]
        public void SetSettings_DoesNotThrow()
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("C02_02").Result).Returns(_room);
            Assert.DoesNotThrowAsync(async () => await _roomService.SetSettingsAsync(_room.RoomName, _settings));
        }
        
        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void SetSettings_WithInvalidRoomName_ThrowsException(string roomName)
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("C02_02").Result).Returns(_room);
            Assert.ThrowsAsync<ArgumentException>(async () => await _roomService.SetSettingsAsync(roomName, _settings));
        }
        
        [Test]
        public void SetSettings_ForNonExistingRoom_ThrowsException()
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("C02_02").Result).Returns(_room);
            Assert.ThrowsAsync<ArgumentException>(async () => await _roomService.SetSettingsAsync("C02_03", _settings));
        }

        [Test]
        public void SetSettings_WithNullSettings_ThrowsException()
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("C02_02").Result).Returns(_room);
            Assert.ThrowsAsync<ArgumentException>(async () => await _roomService.SetSettingsAsync(_room.RoomName, null));
        }
        
        [TestCase(-1, 1, 1,1 )]
        [TestCase(1, -1, 1,1 )]
        [TestCase(1, 1, -1,1 )]
        [TestCase(1, 1, 1,-1 )]

        public void SetSettings_WithInvalidAttributes_ThrowsException(int co2Threshold, int humidityThreshold, int temperatureMargin, float targetTemperature)
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("C02_02").Result).Returns(_room);
            var newSettings = new Settings()
            {
                Co2Threshold = co2Threshold,
                HumidityThreshold = humidityThreshold,
                TemperatureMargin = temperatureMargin,
                TargetTemperature = targetTemperature
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await _roomService.SetSettingsAsync(_room.RoomName, newSettings));
        }
    }
}