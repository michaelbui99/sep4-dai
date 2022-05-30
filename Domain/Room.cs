using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    [Index(nameof(RoomName))]
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        public string RoomName { get; set; }
        public Settings? Settings { get; set; }
        public IEnumerable<ClimateDevice> ClimateDevices { get; set; } = new List<ClimateDevice>();
    }
}