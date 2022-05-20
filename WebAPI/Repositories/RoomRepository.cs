using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.Persistence;
using WebAPI.Util;

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

        public async Task<Room?> GetRoomByNameAsync(string roomName)
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(roomMe => roomMe.ClimateDevices).ThenInclude(device => device.Sensors)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Actuators)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomName== roomName);
        }

        public async Task<IEnumerable<Measurement?>> GetRoomMeasurementsBetweenAsync(long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName)
        {
            var validFrom = DateUtil.GetDateTimeFromUnixTimeSeconds((long) validFromUnixSeconds);
            var validTo = DateUtil.GetDateTimeFromUnixTimeSeconds((long) validToUnixSeconds);
            var measurementsToReturn = new List<Measurement>();

            using (var connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironmentDataWareHouse()))
            {
                const string query ="SELECT * FROM edw.FactMeasurement f " +
                                    "JOIN edw.DimDate d ON f.MD_ID = d.D_ID " +
                                    "JOIN edw.DimTime t ON f.MT_ID = t.TimeKey " +
                                    "JOIN edw.DimRoom r ON f.R_Id = r.R_ID " +
                                    "WHERE CAST(d.Date AS DATETIME) + CAST(t.TimeAltKey AS DATETIME) >= @validFrom " +
                                    "AND CAST(d.Date AS DATETIME) + CAST(t.TimeAltKey AS DATETIME) <= @validTo " +
                                    "AND r.RoomName = @roomName";
                connection.Open();
                await using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@validFrom", validFrom);
                    command.Parameters.AddWithValue("@validTo", validTo);
                    command.Parameters.AddWithValue("@roomName", roomName);

                    var result =  await command.ExecuteReaderAsync();

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            var dateTime = result.GetDateTime(9);
                            dateTime = dateTime + result.GetTimeSpan(19);
                            measurementsToReturn.Add(new Measurement()
                            {
                                Co2 = result.GetInt32(5),
                                Temperature = (float) result.GetDouble(6),
                                Humidity = result.GetInt32(7),
                                Timestamp = dateTime
                            });
                        }
                    }
                }
                connection.Close();
            }

            return measurementsToReturn;
        }
    }
}