using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Services
{
    public interface IMeasurementService
    {
        Task AddMeasurements(string deviceId, IEnumerable<Measurement> measurements);
    }
}