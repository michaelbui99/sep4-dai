using Domain;
using WebAPI.Persistence;

namespace WebAPI.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private AppDbContext _appDbContext;

        public RoomRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    
        public Task<Room> GetRoomByIdAsync(int id)
        {
            return null;
            //var room = _appDbContext;
        }
    }
}

