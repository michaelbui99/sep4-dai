using System.Threading.Tasks;
using Domain;

namespace WebAPI.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> GetMeasurementsByRoomIdAsync(int id);
    }
}