using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;

namespace WebAPI.Services
{
    public interface IDeviceService
    {
        /// <summary>
        /// fetches list of device names
        /// </summary>
        /// <returns> a list of all existing climatedevice names</returns>
        Task<IDictionary<string, string>> GetRoomNamesForDevices();
        /// <summary>
        ///  gets all devices
        /// </summary>
        /// <returns> a list of climatedevices</returns>
        Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync();
        /// <summary>
        /// get a device by Id
        /// </summary>
        /// <param name="deviceId"> the  id of the device you want returned</param>
        /// <exception cref="ArgumentNullException"> on null argument</exception>
        /// <exception cref="ArgumentException"> if device does not exist</exception>
        /// <returns> a climatedevice</returns>
        Task<ClimateDevice> GetDeviceByIdAsync(string deviceId);
        /// <summary>
        ///  adds a new device
        /// </summary>
        /// <param name="device"> the device object you want added</param>
        /// <exception cref="DeviceAlreadyExistsException"> on aslready exists</exception>
        /// <exception cref="ArgumentNullException"> on null argument</exception>
        /// <returns> void</returns>
        Task AddNewDeviceAsync(ClimateDevice device);
    }
}