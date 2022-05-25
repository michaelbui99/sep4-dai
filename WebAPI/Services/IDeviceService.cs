using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Services
{
    public interface IDeviceService
    {
        Task<IDictionary<string, string>> GetRoomNamesForDevices();
        Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync();
        Task<ClimateDevice> GetDeviceByIdAsync(String deviceId);
        Task AddNewDeviceAsync(ClimateDevice device);
    }
}