using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IDeviceRepository
    {
        /// <summary>
        ///  gets all devices
        /// </summary>
        /// <returns> a list of climatedevices</returns>
        Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync();
        /// <summary>
        /// get a device by Id
        /// </summary>
        /// <param name="deviceId"> the  id of the device you want returned</param>
        /// <returns> a climatedevice</returns>
        Task<ClimateDevice> GetDeviceByIdAsync(string deviceId);
        /// <summary>
        ///  adds a new device
        /// </summary>
        /// <param name="device"> the device object you want added</param>
        /// <exception cref="ArgumentException"> error on insert query</exception>
        /// <returns> void</returns>
        Task AddNewDeviceAsync(ClimateDevice device);
    }
}