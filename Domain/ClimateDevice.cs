using System.ComponentModel.DataAnnotations;

namespace Domain;

public class ClimateDevice
{
    [Key]
    public int ClimateDeviceId { get; set; }

    public IEnumerable<Sensor> Sensors { get; set; } = new List<Sensor>();
    public IEnumerable<Actuator> Actuators { get; set; } = new List<Actuator>();
    public IEnumerable<Measurement> Measurements { get; set; } = new List<Measurement>();
    public Settings? Settings { get; set; }
}