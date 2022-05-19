using System;
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

      
    }
}