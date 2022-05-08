using Domain;

namespace WebAPI.Repositories;

public interface IMeasurementRepository
{
    Task<IEnumerable<Measurement>> GetAllAsync();
}