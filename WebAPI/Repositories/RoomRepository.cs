using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(roomMe => roomMe.ClimateDevices).ThenInclude(device => device.Sensors)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Actuators)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomId == id);
        }

        public async Task<Room?> GetRoomByName(string roomName)
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(roomMe => roomMe.ClimateDevices).ThenInclude(device => device.Sensors)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Actuators)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomName== roomName);
        }

        public async Task AddMeasurements(string deviceId, int roomId, IEnumerable<Measurement> measurements)
        {
            using (SqlConnection connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironment()))
            {
                string query =
                    "INSERT INTO dbo.Measurements(Timestamp, Temperature, Humidity, Co2, ClimateDeviceId) " +
                    "VALUES (@timestamp,@temperature,@humidity,@co2,@deviceId)";
                connection.Open();
                foreach (var measurement in measurements)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@timestamp", measurement.Timestamp);
                        command.Parameters.AddWithValue("@temperature", measurement.Temperature);
                        command.Parameters.AddWithValue("@humidity", measurement.Humidity);
                        command.Parameters.AddWithValue("@co2", measurement.Co2);
                        command.Parameters.AddWithValue("@deviceId", deviceId);

                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            throw new ArgumentException("Could not insert new query");
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}