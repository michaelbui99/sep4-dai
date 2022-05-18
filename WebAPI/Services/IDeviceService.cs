using Domain;

namespace WebAPI.Services;

public interface IDeviceService
{
    Task<ClimateDevice> GetDeviceById(String deviceId);
    Task AddNewDevice(ClimateDevice device);

}