using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Actuator
{
    [Key]
    public int ActuatorId { get; set; }

    public string? Type { get; set; }
}
