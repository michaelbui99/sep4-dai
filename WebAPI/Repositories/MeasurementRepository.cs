using Domain;
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
        return _dbContext.Measurements;
    }
}