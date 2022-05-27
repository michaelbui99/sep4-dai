using Domain;

namespace WebAPI.Services
{
    public interface IDeviceService
    {
        Task<IDictionary<string, string>> GetRoomNamesForDevices();
        Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync();
        Task<ClimateDevice> GetDeviceByIdAsync(string deviceId);
        Task AddNewDeviceAsync(ClimateDevice device);
    }
}