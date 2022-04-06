using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Sensor
{
    [Key]
    public int SensorId { get; set; }
}