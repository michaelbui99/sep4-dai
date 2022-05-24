using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Services
{
    public class MeasurementService : IMeasurementService
    {
        private IMeasurementRepository _measurementRepository;
        private IDeviceService _deviceService;

        public MeasurementService(IMeasurementRepository measurementRepository, IDeviceService deviceService)
        {
            _measurementRepository = measurementRepository;
            _deviceService = deviceService;
        }

        public async Task AddMeasurements(string deviceId, IEnumerable<Measurement> measurements)
        {
            if (String.IsNullOrEmpty(deviceId) || String.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException("Invalid deviceid provided");
            }

            if (measurements is null)
            {
                throw new ArgumentException("Measurements cannot be null");
            }

            try
            {
                await _deviceService.GetDeviceByIdAsync(deviceId);
            }
            catch (ArgumentException e)
            {
                try
                {
                    Console.WriteLine("miaw");
                    await _deviceService.AddNewDeviceAsync(new ClimateDevice() {ClimateDeviceId = deviceId});
                    await _measurementRepository.AddMeasurements(deviceId, measurements);
                    return;
                }
                catch (DeviceAlreadyExistsException e1)
                {
                    Console.WriteLine(e1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await _measurementRepository.AddMeasurements(deviceId, measurements);
        }
    }
}