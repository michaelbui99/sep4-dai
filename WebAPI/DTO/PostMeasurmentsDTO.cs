using System.Collections.Generic;
using Domain;

namespace WebAPI.DTO
{

    public class PostMeasurmentsDTO
    {
        public string DeviceId { get; set; }
        public IList<Measurement> Measurements { get; set; } = new List<Measurement>();
    }
}