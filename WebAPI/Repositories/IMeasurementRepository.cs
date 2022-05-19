using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{

    public interface IMeasurementRepository
    {
        Task<IEnumerable<Measurement>> GetAllAsync();
        Task<IEnumerable<Measurement>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<Measurement>> GetByRoomNameAsync(string roomName);
        Task AddMeasurements(string deviceId, IEnumerable<Measurement> measurements);
    }
}