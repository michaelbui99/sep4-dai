﻿using System;
using System.Collections.Generic;
using Domain;
using Moq;
using NUnit.Framework;
using WebAPI.Repositories;
using WebAPI.Services;

namespace Sep4Test.RoomServiceTests
{
    [TestFixture]
    public class GetRoomByNameTest
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
                Measurements = _measurements,
                Settings = _settings
            };
            _climateDevices = new List<ClimateDevice>();
            _climateDevices.Add(_climateDevice);
            
            _room = new Room()
            {
                RoomId = 1,
                RoomName = "c02_02",
                ClimateDevices = _climateDevices,
                Settings = _settings
            };

            _roomRepository = new Mock<IRoomRepository>();
            _measurementRepository = new Mock<IMeasurementRepository>();
            _roomService = new RoomService(_roomRepository.Object, _measurementRepository.Object);
        }

        [Test]
        public void GetRoomByName_DoesNotThrow()
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("c02_02").Result).Returns(_room);
            Assert.DoesNotThrowAsync(async () => await _roomService.GetRoomByNameAsync(_room.RoomName));
        }

        [Test]
        public void GetRoomByName_Returns_Correct_Room()
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("c02_02").Result).Returns(_room);
            Assert.AreEqual(_room, _roomService.GetRoomByNameAsync("c02_02").Result);
        }
        
        [TestCase(null)]
        [TestCase("")]
        public void GetRoomByName_With_ImvalidRoomName_ThrowArgumentException(string roomName)
        {
            _roomRepository.Setup<Room>(repository => repository.GetRoomByNameAsync("c02_02").Result).Returns(_room);
            Assert.ThrowsAsync<ArgumentException>(async () => await _roomService.GetRoomByNameAsync(roomName));
        }
    }
}