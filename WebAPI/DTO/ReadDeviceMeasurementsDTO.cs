using System.Collections;
using System.Collections.Generic;
using Domain;

namespace WebAPI.DTO
{
    public class ReadDeviceMeasurementsDTO
    {

        public string DeviceId { get; set; }
        public IEnumerable<Measurement> Measurements { get; set; }
    }
}