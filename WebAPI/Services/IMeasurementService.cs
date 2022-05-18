using Domain;

namespace WebAPI.Services;

public interface IMeasurementService
{
    
    Task AddMeasurements(string deviceId, IEnumerable<Measurement> measurements);
}