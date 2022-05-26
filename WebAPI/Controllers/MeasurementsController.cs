using System;
using System.Collections;
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
        
        /// <summary>
        /// Method to add measurements from the post request body,
        /// convert it and added to the internal storage.
        /// </summary>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <exception>if the measurement is not properly formatted return bad request</exception>
        /// <param name="measurements"></param>
        /// <returns>ok meaning that the measurement was added</returns>

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Measurement>>> AddMeasurements(
            [FromBody]
            PostMeasurmentsDTO measurements)
        {
            try
            {
                _logger.LogInformation($"Received: {JsonSerializer.Serialize(measurements)}");
                var measurementsWithoutDuplicates = RemoveDuplicateMeasurements(measurements.Measurements);
                await _measurementService.AddMeasurements(measurements.DeviceId, measurementsWithoutDuplicates);
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

        /// <summary>
        /// Removes all duplicate measurements
        /// </summary>
        /// <remarks>This is an not-in-place implementation where we return a new IEnumerable without duplicates</remarks>
        /// <param name="measurements">Measurements to process</param>
        /// <returns>IEnumerable of measurements without duplicate measurements</returns>
        private IEnumerable<Measurement> RemoveDuplicateMeasurements(IEnumerable<Measurement> measurements)
        {
            var measurementsToReturn = new List<Measurement>();
            var encounteredTimestamps = new Dictionary<DateTime, DateTime>();

            foreach (var measurement in measurements)
            {
                if (encounteredTimestamps.ContainsKey(measurement.Timestamp))
                {
                    continue;
                }

                measurementsToReturn.Add(measurement);
                encounteredTimestamps[measurement.Timestamp] = measurement.Timestamp;
            }

            return measurementsToReturn;
        }
    }
}