using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMeasurementRepository _measurementRepository;

        public RoomService(IRoomRepository roomRepository, IMeasurementRepository measurementRepository)
        {
            _roomRepository = roomRepository;
            _measurementRepository = measurementRepository;
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                throw new ArgumentException("Room was null");
            }

            return room;
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id)
        {
            // var room = await GetRoomByIdAsync(id);
            //
            // var roomDeviceIds = new List<int>();
            // foreach (var roomClimateDevice in room.ClimateDevices)
            // {
            //     roomDeviceIds.Add(roomClimateDevice.ClimateDeviceId);
            // }

            // IList<Measurement> measurements = room.ClimateDevices
            //     .SelectMany(device => device.Measurements).ToList();

            return await _measurementRepository.GetAllAsync();
        }

        public async Task AddMeasurements(int deviceId, int roomId, IEnumerable<Measurement> measurements)
        {
            await _roomRepository.AddMeasurements(deviceId, roomId, measurements);
        }
    }
}