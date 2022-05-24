using Domain;

namespace WebAPI.DTO
{
    public class GetAllDevicesDTO
    {
        public string ClimateDeviceId { get; set; }
        public Settings? Settings { get; set; }
        public string RoomName { get; set; }
    }
}