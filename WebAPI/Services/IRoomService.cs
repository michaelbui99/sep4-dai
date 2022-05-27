using Domain;

namespace WebAPI.Services
{
    public interface IRoomService
    {
        /// <summary>
        /// Add new instance of Room object
        /// </summary>
        /// <param name="rName">The name of  the new created room</param>
        /// <exception cref="ArgumentException"> On invalid parameter</exception>
        /// <returns></returns>
        Task CreateNewRoomAsync(string rName);
        /// <summary>
        /// Get all existing Rooms
        /// </summary>
        /// <returns>A list of Room object</returns>
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        /// <summary>
        /// Get a Room object by given parameter
        /// </summary>
        /// <param name="id">The id of the targeted room</param>
        /// <exception cref="ArgumentException">On invalid parameter</exception>
        /// <returns>A Room object</returns>
        Task<Room> GetRoomByIdAsync(int id);
        /// <summary>
        /// Get a list of Measurement for a room by a given parameter
        /// </summary>
        /// <param name="id">The id of the targeted room</param>
        /// <exception cref="ArgumentException">On invalid parameter</exception>
        /// <returns>A list of Measurement object</returns>
        Task<IEnumerable<Measurement>> GetMeasurementsByRoomIdAsync(int id);
        /// <summary>
        /// Get a list of Measurement for a room by a given parameter
        /// </summary>
        /// <param name="roomName">The name of the targeted room</param>
        /// <exception cref="ArgumentException">On invalid parameter</exception>
        /// <returns>A list of Measurement object</returns>
        Task<IEnumerable<Measurement>> GetMeasurementsByRoomNameAsync(string roomName);
        /// <summary>
        /// Add new list Measurement object
        /// </summary>
        /// <param name="deviceId">The device id witch the measurement belong to</param>
        /// <param name="roomName">The Room name witch the measurement belong to</param>
        /// <param name="measurements">The new list of Measurement object</param>
        /// <returns></returns>
        [Obsolete("AddMeasurementsAsync is deprecated, please use AddMeasurements from IMeasurementRepository instead.")]
        Task AddMeasurementsAsync(string deviceId, string roomName, IEnumerable<Measurement> measurements);
        /// <summary>
        /// Get a list of room Measurements based on given parameters 
        /// </summary>
        /// <param name="validFromUnixSeconds">Start date for query</param>
        /// <param name="validToUnixSeconds">End date for query</param>
        /// <param name="roomName">The name of the targeted room</param>
        /// <exception cref="ArgumentException">On invalid roomName parameter</exception>
        /// <exception cref="KeyNotFoundException">On invalid valid from and valid to parameters</exception>
        /// <returns>A Dictionary/HashMap with a key of value string and value of a list of Measurement object</returns>
        Task<IDictionary<string, IList<Measurement?>>> GetRoomMeasurementsBetweenAsync(long? validFromUnixSeconds,
            long? validToUnixSeconds, string roomName);
        /// <summary>
        /// Get a Room object based on given parameter
        /// </summary>
        /// <param name="name">The name of the targeted room</param>
        /// <exception cref="ArgumentException">On invalid parameter</exception>
        /// <returns>A Room object</returns>
        Task<Room> GetRoomByNameAsync(string name);
        /// <summary>
        /// Update an already existing room Device object
        /// </summary>
        /// <param name="roomName">The name of the room which the device belong to</param>
        /// <param name="deviceId">The Id of the targeted device</param>
        /// <exception cref="ArgumentException">On invalid parameters</exception>
        /// <returns></returns>
        Task UpdateRoomDevicesAsync(string roomName, string deviceId);
        /// <summary>
        /// Add new Settings to a room
        /// </summary>
        /// <param name="roomName">The name of the targeted room</param>
        /// <param name="settings">The new Settings</param>
        /// <returns></returns>
        Task SetSettingsAsync(string roomName, Settings settings);
    }
}