using Domain;
using Microsoft.EntityFrameworkCore;
using WebAPI.Persistence;

namespace WebAPI.Repositories;

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

    public async Task<IEnumerable<Measurement>> GetByIdAsync(int roomId)
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
}