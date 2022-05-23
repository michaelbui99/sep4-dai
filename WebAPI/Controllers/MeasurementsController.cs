using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.DTO;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class MeasurementsController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly ILogger _logger;

        public MeasurementsController(IMeasurementService measurementService, ILogger<MeasurementsController> logger)
        {
            _measurementService = measurementService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Measurement>>> AddMeasurements(
            [FromBody] PostMeasurmentsDTO measurements)
        {
            try
            {
                _logger.LogInformation($"Received: {JsonSerializer.Serialize(measurements)}");
                await _measurementService.AddMeasurements(measurements.DeviceId, measurements.Measurements);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
