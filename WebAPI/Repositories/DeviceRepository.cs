using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.Persistence;

namespace WebAPI.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext _appDbContext;

        public DeviceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<ClimateDevice>> GetAllDevicesAsync()
        {
            return await _appDbContext.Devices?.Include(device => device.Measurements).Include(
                device => device.Settings).ToListAsync();
        }

        public async Task<ClimateDevice> GetDeviceByIdAsync(string deviceId)
        {
            return await _appDbContext.Devices?.Include(device => device.Measurements).Include(
                device => device.Settings).FirstOrDefaultAsync(device => device.ClimateDeviceId.ToLower() == deviceId.ToLower());
        }

        public async Task AddNewDeviceAsync(ClimateDevice device)
        {
            using (SqlConnection connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironment()))
            {
                string query =
                    "INSERT INTO dbo.Devices(ClimateDeviceId, SettingsSettingId, RoomId) " +
                    "VALUES (@ClimateDeviceId, @SettingsId,@RoomId)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClimateDeviceId", device.ClimateDeviceId.ToLower());
                    command.Parameters.AddWithValue("@SettingsId", 0);
                    command.Parameters.AddWithValue("@RoomId", 0);

                    int result = command.ExecuteNonQuery();

                    if (result < 0)
                    {
                        throw new ArgumentException("Could not insert new query");
                    }
                }

                await connection.CloseAsync();
            }
        }

        public async Task<IEnumerable<ClimateDevice>> GetAllDevicesExcludingMeasurementsAsync()
        {
            return await _appDbContext.Devices?.Include(
                device => device.Settings).ToListAsync();
        }
    }
}