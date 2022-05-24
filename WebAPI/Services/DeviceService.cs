using System;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Services
{
    public class DeviceService : IDeviceService
    {
        private IDeviceRepository _deviceRepository;
        private IRoomService _roomService;

        public DeviceService(IDeviceRepository deviceRepository, IRoomService roomService)
        {
            _deviceRepository = deviceRepository;
            _roomService = roomService;
        }

        public async Task<IDictionary<string, string>> GetRoomNamesForDevices()
        {
            var roomNameMapping = new Dictionary<string, string>();
            var roomList = await _roomService.GetAllRoomsAsync();
            foreach (var room in roomList)
            {
                foreach (var roomClimateDevice in room.ClimateDevices)
                {
                    roomNameMapping[roomClimateDevice.ClimateDeviceId] = room.RoomName;
                }
            }

            return roomNameMapping;
        }

        public async Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync()
        {
            return await _deviceRepository.GetAllDevicesAsync();
        }

        public async Task<ClimateDevice> GetDeviceByIdAsync(string deviceId)
        {
            if (!string.IsNullOrWhiteSpace(deviceId) || !string.IsNullOrWhiteSpace(deviceId))
            {
                var climateDevice = await _deviceRepository.GetDeviceByIdAsync(deviceId);

                if (climateDevice == null)
                {
                    throw new ArgumentException($"No device with id: {deviceId} exists");
                }

                return climateDevice;
            }

            throw new ArgumentNullException("deviceId can't be null");
        }

        public async Task AddNewDeviceAsync(ClimateDevice device)
        {
            if (device != null)
            {
                try
                {
                    await GetDeviceByIdAsync(device.ClimateDeviceId);
                    throw new DeviceAlreadyExistsException($"Device with id: {device.ClimateDeviceId} exists");
                }
                catch (ArgumentException e)
                {
                    await _deviceRepository.AddNewDeviceAsync(device);
                }
            }
            else
            {
                throw new ArgumentNullException("device can't be null");
            }
        }
    }
}