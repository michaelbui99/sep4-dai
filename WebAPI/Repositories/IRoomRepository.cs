using Domain;

namespace WebAPI.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);
        Task<Room?> GetRoomByNameAsync(string roomName);
        Task<IEnumerable<Measurement?>> GetRoomMeasurementsBetweenAsync(long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName);
    }
}