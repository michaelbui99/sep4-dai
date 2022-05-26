using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMeasurementRepository _measurementRepository;
        private static readonly string ROOM_NAME_FORMAT = @"[A-Z][0-9][0-9]_[0-9][0-9][a-z]?$";

        public RoomService(IRoomRepository roomRepository, IMeasurementRepository measurementRepository)
        {
            _roomRepository = roomRepository;
            _measurementRepository = measurementRepository;
        }

        public async Task CreateNewRoomAsync(string rName)
        {
            if (await RoomExists(rName))
            {
                throw new ArgumentException($"Room with the name: {rName} already in the system");
            }

            if (string.IsNullOrEmpty(rName) || string.IsNullOrWhiteSpace(rName) || !Regex.IsMatch(rName, ROOM_NAME_FORMAT))
            {
                throw new ArgumentException("Invalid Room name");
            }

            await _roomRepository.CreateNewRoomAsync(rName);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllRoomsAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                throw new ArgumentException($"No room with id: {id} exists");
            }

            return room;
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id)
        {
            if (!(await RoomExists(id)))
            {
                throw new ArgumentException($"No room with id: {id} exists");
            }

            return await _measurementRepository.GetByRoomIdAsync(id);
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsByRoomNameAsync(string roomName)
        {
            if (!(await RoomExists(roomName)))
            {
                throw new ArgumentException($"No room with id: {roomName} exists");
            }
            
            if (String.IsNullOrEmpty(roomName) || String.IsNullOrWhiteSpace(roomName))
            {
                throw new ArgumentException("Invalid deviceid provided");
            }

            return await _measurementRepository.GetByRoomNameAsync(roomName);
        }

        public async Task AddMeasurementsAsync(string deviceId, string roomName, IEnumerable<Measurement> measurements)
        {
            var existingRoom = await _roomRepository.GetRoomByNameAsync(roomName);

            if (!(await RoomExists(roomName)))
            {
                throw new ArgumentException($"No room with id: {roomName} exists");
            }

            if (!ClimateDeviceExists(existingRoom, deviceId))
            {
                throw new ArgumentException($"No device with id: {deviceId} exists");
            }

            await _measurementRepository.AddMeasurements(deviceId, measurements);
        }

        public async Task<IDictionary<string, IList<Measurement?>>> GetRoomMeasurementsBetweenAsync(
            long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName)
        {
            
            if (String.IsNullOrEmpty(roomName) || String.IsNullOrWhiteSpace(roomName))
            {
                throw new ArgumentException("Invalid deviceid provided");
            }
            
            if (validFromUnixSeconds == null && validToUnixSeconds != null)
            {
                throw new KeyNotFoundException($"ValidFrom cant be null");
            }

            if (validFromUnixSeconds != null && validToUnixSeconds == null)
            {
                throw new KeyNotFoundException($"ValidTo cant be null");
            }

            if (!(await RoomExists(roomName)))
            {
                throw new ArgumentException($"No room with name: {roomName} exists");
            }

            return await _roomRepository.GetRoomMeasurementsBetweenAsync(validFromUnixSeconds, validToUnixSeconds,
                roomName);
        }

        public async Task<Room> GetRoomByNameAsync(string name)
        {
            if (!await RoomExists(name))
            {
                throw new ArgumentException($"No room with this name exists: {name}");
            }

            return await _roomRepository.GetRoomByNameAsync(name);
        }

        public async Task UpdateRoomDevicesAsync(string roomName, string deviceId)
        {
            if (!await RoomExists(roomName))
            {
                throw new ArgumentException($"No room with this name exists: {roomName}");
            }

            if (!await ClimateDeviceExists(roomName, deviceId))
            {
                throw new ArgumentException($"No device with this id exists: {deviceId}");
            }

            await _roomRepository.UpdateRoomDevicesAsync(roomName, deviceId);
        }

        public async Task SetSettingsAsync(string roomName, Settings settings)
        {
            if (!await RoomExists(roomName))
            {
                throw new ArgumentException($"No room with this name exists: {roomName}");
            }

            await _roomRepository.SetSettingsAsync(roomName, settings);
        }

        private bool ClimateDeviceExists(Room room, string deviceId)
        {
            return room.ClimateDevices.Any(roomClimateDevice => roomClimateDevice.ClimateDeviceId == deviceId);
        }
        
        private async Task<bool> ClimateDeviceExists(string roomName, string deviceId)
        {
            var rooms = await GetAllRoomsAsync();
            foreach (var room in rooms)
            {
                if (room.ClimateDevices.Any(device => device.ClimateDeviceId == deviceId))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> RoomExists(int roomId)
        {
            var existingRoom = await _roomRepository.GetRoomByIdAsync(roomId);
            if (existingRoom == null)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> RoomExists(string roomName)
        {
            var existingRoom = await _roomRepository.GetRoomByNameAsync(roomName);
            if (existingRoom == null)
            {
                return false;
            }

            return true;
        }
    }
}