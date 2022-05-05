using Domain;

namespace WebAPI.Repositories;

public class RoomRepository : IRoomRepository
{
    public Task<Room> GetRoomByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}