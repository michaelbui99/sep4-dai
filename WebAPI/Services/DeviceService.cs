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

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<ClimateDevice> GetDeviceById(string deviceId)
        {
            var climateDevice = await _deviceRepository.GetDeviceById(deviceId);

            if (climateDevice == null)
            {
                throw new ArgumentException($"No device with id: {deviceId} exists");
            }

            return climateDevice;
        }

        public async Task AddNewDevice(ClimateDevice device)
        {
            try
            {
                await GetDeviceById(device.ClimateDeviceId);
                throw new DeviceAlreadyExistsException($"Device with id: {device.ClimateDeviceId} exists");
            }
            catch (ArgumentException e)
            {
                await _deviceRepository.AddNewDevice(device);
            }
        }
    }
}