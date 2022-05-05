using System;
using System.Threading.Tasks;
using Domain;
using WebAPI.Repositories;

namespace WebAPI.Repositories;

public class RoomRepository : IRoomRepository
{
    public Task<Room> GetRoomByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}