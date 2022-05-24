using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room?>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);
        Task<Room?> GetRoomByNameAsync(string roomName);
        Task<IDictionary<string, IList<Measurement?>>> GetRoomMeasurementsBetweenAsync(long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName);

        Task UpdateRoomDevicesAsync(string roomName, string deviceId);
        
        
    }
}