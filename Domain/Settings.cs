using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Settings
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        public int Co2Threshold { get; set; }

        [Range(0, 100)]
        [Required]
        public int HumidityThreshold { get; set; }

        [Required]
        public float TargetTemperature { get; set; }

        [Required]
        public int TemperatureMargin { get; set; }
    }
}