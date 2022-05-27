using Domain;
using Domain.Exceptions;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepository _measurementRepository;
        private readonly IDeviceService _deviceService;

        public MeasurementService(IMeasurementRepository measurementRepository, IDeviceService deviceService)
        {
            _measurementRepository = measurementRepository;
            _deviceService = deviceService;
        }

        public async Task AddMeasurements(string deviceId, IEnumerable<Measurement> measurements)
        {
            if (string.IsNullOrEmpty(deviceId) || string.IsNullOrWhiteSpace(deviceId))
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
                var measurementsWithoutDuplicates = await RemoveDuplicates(deviceId, measurements);
                await _measurementRepository.AddMeasurements(deviceId, measurementsWithoutDuplicates);
            }
            catch (ArgumentException e)
            {
                try
                {
                    await _deviceService.AddNewDeviceAsync(new ClimateDevice() {ClimateDeviceId = deviceId});
                    await _measurementRepository.AddMeasurements(deviceId, measurements);
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
        }

        private async Task<IEnumerable<Measurement>> RemoveDuplicates(string deviceId,
            IEnumerable<Measurement> measurements)
        {
            var device = await _deviceService.GetDeviceByIdAsync(deviceId);
            var encounteredTimestamps = new Dictionary<string, string>();
            var measurementsToAdd = new List<Measurement>();

            foreach (var deviceMeasurement in device.Measurements)
            {
                encounteredTimestamps[deviceMeasurement.Timestamp.ToString()] = deviceMeasurement.Timestamp.ToString();
            }

            foreach (var newMeasurement in measurements)
            {
                if (encounteredTimestamps.ContainsKey(newMeasurement.Timestamp.ToString()))
                {
                    continue;
                }

                measurementsToAdd.Add(newMeasurement);
            }

            return measurementsToAdd;
        }
    }
}