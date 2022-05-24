using System;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync();
        Task<ClimateDevice> GetDeviceByIdAsync(String deviceId);
        Task AddNewDeviceAsync(ClimateDevice device);
    }
}