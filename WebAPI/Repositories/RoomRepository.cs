using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.Persistence;
using WebAPI.Util;

namespace WebAPI.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _appDbContext;

        public RoomRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateNewRoomAsync(string rName)
        {
            var roomToAdd = new Room()
            {
                RoomName = rName,
                ClimateDevices = new List<ClimateDevice>(),
                //Def settings
                Settings = new Settings()
                {
                    Co2Threshold = 900,
                    TargetTemperature = 20,
                    HumidityThreshold = 30,
                    TemperatureMargin = 3
                }
            };
            await _appDbContext.Rooms.AddAsync(roomToAdd);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room?>> GetAllRoomsAsync()
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomId == id)!;
        }

        public async Task<Room?> GetRoomByNameAsync(string roomName)
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomName == roomName);
        }

        public async Task<IDictionary<string, IList<Measurement?>>> GetRoomMeasurementsBetweenAsync(
            long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName)
        {
            var validFrom = DateUtil.GetDateTimeFromUnixTimeSeconds((long) validFromUnixSeconds);
            var validTo = DateUtil.GetDateTimeFromUnixTimeSeconds((long) validToUnixSeconds);
            var deviceMeasurementMap = new Dictionary<string, IList<Measurement>>();

            using (var connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironmentDataWareHouse()))
            {
                const string connectionSetting = "SET ARITHABORT ON";
                const string query = "SELECT * FROM edw.FactMeasurement f " +
                                     "JOIN edw.DimDate d ON f.MD_ID = d.D_ID " +
                                     "JOIN edw.DimTime t ON f.MT_ID = t.TimeKey " +
                                     "JOIN edw.DimRoom r ON f.R_Id = r.R_ID " +
                                     "JOIN edw.DimClimateDevice cd on f.C_ID = cd.C_ID " +
                                     "WHERE CAST(d.Date AS DATETIME) + CAST(t.TimeAltKey AS DATETIME) >= @validFrom " +
                                     "AND CAST(d.Date AS DATETIME) + CAST(t.TimeAltKey AS DATETIME) <= @validTo " +
                                     "AND r.RoomName = @roomName";
                connection.Open();
                await using (var settingCommand = new SqlCommand(connectionSetting, connection))
                {
                    settingCommand.ExecuteNonQuery();
                }

                await using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@validFrom", validFrom);
                    command.Parameters.AddWithValue("@validTo", validTo);
                    command.Parameters.AddWithValue("@roomName", roomName);
                    command.CommandTimeout = 120;
                    var result = await command.ExecuteReaderAsync();

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            var dateTime = result.GetDateTime(9);
                            dateTime = dateTime + result.GetTimeSpan(19);
                            var ClimateDeviceId = result.GetString(30);

                            if (!deviceMeasurementMap.ContainsKey(ClimateDeviceId))
                            {
                                deviceMeasurementMap[ClimateDeviceId] = new List<Measurement>();
                            }

                            var measurement = new Measurement()
                            {
                                Co2 = result.GetInt32(5),
                                Temperature = (float) result.GetDouble(6),
                                Humidity = result.GetInt32(7),
                                Timestamp = dateTime
                            };

                            deviceMeasurementMap[ClimateDeviceId].Add(measurement);
                        }
                    }

                    command.Connection.Close();
                }

                connection.Close();
            }

            return deviceMeasurementMap;
        }

        public async Task UpdateRoomDevicesAsync(string roomName, string deviceId)
        {
            using (var connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironment()))
            {
                const string query =
                    "update dbo.Devices set RoomId = (select RoomId from dbo.Rooms where RoomName = @roomName) where ClimateDeviceId = @deviceId";
                connection.Open();
                await using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@roomName", roomName);
                    command.Parameters.AddWithValue("@deviceId", deviceId);

                    var result = await command.ExecuteNonQueryAsync();

                    if (result < 0)
                    {
                        throw new ArgumentException("Could not update new query");
                    }
                }

                await connection.CloseAsync();
                await UpdateDeviceSetting(roomName, deviceId);
            }
        }

        public async Task SetSettingsAsync(string roomName, Settings settings)
        {
            var room = await GetRoomByNameAsync(roomName);
            room.Settings = settings;

            foreach (var roomClimateDevice in room.ClimateDevices)
            {
                roomClimateDevice.Settings = settings;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRoomsExcludingDeviceMeasurementsAsync()
        {
            return await _appDbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRoomsExcludingDevicesAsync()
        {
            return await _appDbContext.Rooms.ToListAsync();
        }

        private async Task UpdateDeviceSetting(string roomName, string deviceId)
        {
            using (var connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironment()))
            {
                const string query =
                    "update dbo.Devices set SettingsSettingId  = (select SettingsSettingId from dbo.Rooms where RoomName = @roomName) where ClimateDeviceId = @deviceId";
                connection.Open();
                await using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@roomName", roomName);
                    command.Parameters.AddWithValue("@deviceId", deviceId);

                    var result = await command.ExecuteNonQueryAsync();

                    if (result < 0)
                    {
                        throw new ArgumentException("Could not update new query");
                    }
                }

                await connection.CloseAsync();
            }
        }
    }
}