using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);
    }
}