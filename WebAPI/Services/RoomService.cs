using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    public class RoomService : IRoomService
    {
        private IRoomRepository roomRepository;


        public RoomService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public Task<Room> GetRoomByIdAsync(int id)
        {
            return roomRepository.GetRoomByIdAsync(id);
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id)
        {
            Room room = await roomRepository.GetRoomByIdAsync(id);
            IList<Measurement> measurements = new List<Measurement>();

            foreach (var device in room.ClimateDevices)
            {
                foreach (var measurement in device.Measurements)
                {
                    measurements.Add(measurement);
                }
            }
            return measurements.AsEnumerable();
        }
    }
}