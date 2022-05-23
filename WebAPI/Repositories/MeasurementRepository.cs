﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.Persistence;

namespace WebAPI.Repositories
{

    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly AppDbContext _dbContext;


        public MeasurementRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Measurement>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Measurement>> GetByRoomIdAsync(int roomId)
        {
            var room = await _dbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(roomMe => roomMe.ClimateDevices).ThenInclude(device => device.Sensors)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Actuators)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomId == roomId);

            return room.ClimateDevices
                .SelectMany(device => device.Measurements).ToList();
        }

        public async Task<IEnumerable<Measurement>> GetByRoomNameAsync(string roomName)
        {
            var room = await _dbContext.Rooms?.Include(roomSettings => roomSettings.Settings)
                .Include(roomMe => roomMe.ClimateDevices).ThenInclude(device => device.Sensors)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Actuators)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Measurements)
                .Include(room => room.ClimateDevices).ThenInclude(device => device.Settings)
                .FirstOrDefaultAsync(room => room.RoomName == roomName);

            return room.ClimateDevices
                .SelectMany(device => device.Measurements).ToList();
        }
        
        public async Task AddMeasurements(string deviceId, IEnumerable<Measurement> measurements)
        {
            using (SqlConnection connection =
                   new SqlConnection(ConnectionStringGenerator.GetConnectionStringFromEnvironment()))
            {
                var query =
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

                        var result = command.ExecuteNonQuery();

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