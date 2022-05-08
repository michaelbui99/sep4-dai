using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);
        Task AddMeasurements(int deviceId, int roomId, IEnumerable<Measurement> measurements);
    }
}