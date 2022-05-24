using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id);
        Task<IEnumerable<Measurement>> GetMeasurementsByRoomNameAsync(string roomName);
        
        [Obsolete("AddMeasurementsAsync is deprecated, please use AddMeasurements from IMeasurementRepository instead.")]
        Task AddMeasurementsAsync(string deviceId, string roomName, IEnumerable<Measurement> measurements);
        Task<IDictionary<string, IList<Measurement?>>> GetRoomMeasurementsBetweenAsync(long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName);

        Task<Room> GetRoomByNameAsync(string name);
    }
}