using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id);
        Task AddMeasurements(int deviceId, IEnumerable<Measurement> measurements);

    }
}