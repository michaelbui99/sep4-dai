using System;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IDeviceRepository
    {
        Task<ClimateDevice> GetDeviceById(String deviceId);
        Task AddNewDevice(ClimateDevice device);
    }
}