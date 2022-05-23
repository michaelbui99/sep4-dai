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
            if (!string.IsNullOrWhiteSpace(deviceId) || !string.IsNullOrWhiteSpace(deviceId))
            {
                var climateDevice = await _deviceRepository.GetDeviceById(deviceId);

                if (climateDevice == null)
                {
                    throw new ArgumentException($"No device with id: {deviceId} exists");
                }

                return climateDevice;
            }

            throw new ArgumentNullException("deviceId can't be null");
        }

        public async Task AddNewDevice(ClimateDevice device)
        {
            if (device != null)
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
            else
            {
                throw new ArgumentNullException("device can't be null");
            }
        }
    }
}