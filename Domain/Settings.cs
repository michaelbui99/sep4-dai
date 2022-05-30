using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    [Index(nameof(SettingId))]
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