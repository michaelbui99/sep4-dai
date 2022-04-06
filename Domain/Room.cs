using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Room
{
    [Key]
    public int RoomId { get; set; }

    public Settings? Settings { get; set; }
    public IEnumerable<ClimateDevice> ClimateDevices { get; set; } = new List<ClimateDevice>();
}