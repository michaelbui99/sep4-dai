using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;


        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public Task<Room> GetRoomByIdAsync(int id)
        {
            return _roomRepository.GetRoomByIdAsync(id);
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id)
        {
            Room room = await _roomRepository.GetRoomByIdAsync(id);

            IList<Measurement> measurements = room.ClimateDevices
                .SelectMany(device => device.Measurements).ToList();

            return measurements.AsEnumerable();
        }
    }
}