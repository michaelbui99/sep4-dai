using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync();
        Task<ClimateDevice> GetDeviceByIdAsync(string deviceId);
        Task AddNewDeviceAsync(ClimateDevice device);
    }
}