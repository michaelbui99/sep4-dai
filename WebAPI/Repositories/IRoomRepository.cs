using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebAPI.Repositories
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Add new instance of Room object
        /// </summary>
        /// <param name="rName">The name of  the new created room</param>
        /// <returns></returns>
        Task CreateNewRoomAsync(string rName);
        /// <summary>
        /// Get all existing Rooms
        /// </summary>
        /// <returns>A list of Room object</returns>
        Task<IEnumerable<Room?>> GetAllRoomsAsync();
        /// <summary>
        /// Get a Room object by given parameter
        /// </summary>
        /// <param name="id">The id of the targeted room</param>
        /// <returns>A Room object</returns>
        Task<Room?> GetRoomByIdAsync(int id);
        /// <summary>
        /// Get a Room object based on given parameter
        /// </summary>
        /// <param name="roomName">The name of the targeted room</param>
        /// <returns>A Room object</returns>
        Task<Room?> GetRoomByNameAsync(string roomName);
        /// <summary>
        /// Get a list of room Measurements based on given parameters 
        /// </summary>
        /// <param name="validFromUnixSeconds">Start date for query</param>
        /// <param name="validToUnixSeconds">End date for query</param>
        /// <param name="roomName">The name of the targeted room</param>
        /// <returns>A Dictionary/HashMap with a key of value string and value of a list of Measurement object</returns>
        Task<IDictionary<string, IList<Measurement?>>> GetRoomMeasurementsBetweenAsync(long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName);
        /// <summary>
        /// Update an already existing room Device object
        /// </summary>
        /// <param name="roomName">The name of the room which the device belong to</param>
        /// <param name="deviceId">The Id of the targeted device</param>
        /// <returns></returns>
        Task UpdateRoomDevicesAsync(string roomName, string deviceId);
        /// <summary>
        /// Set new Setting object to a room 
        /// </summary>
        /// <param name="roomName">The name of the targeted room</param>
        /// <param name="settings">The new Setting object</param>
        /// <returns></returns>
        Task SetSettingsAsync(string roomName, Settings settings);

        Task<IEnumerable<Room>> GetAllRoomsExcludingDevicesAsync();
    }
}