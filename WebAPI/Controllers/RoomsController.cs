using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.DTO;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomService roomService;
        private readonly ILogger _logger;

        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
        {
            this.roomService = roomService;
            _logger = logger;
        }

        [HttpGet("{roomId:int}/measurements")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurmentByRoomId([FromRoute] int roomId)
        {
            try
            {
                IEnumerable<Measurement> measurements = await roomService.GetMeasurementsByRoomIdAsync(roomId);
                return Ok(measurements);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get measurements: {e}", e);
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("{roomName}/measurements")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurmentByRoomName([FromRoute] string roomName)
        {
            try
            {
                IEnumerable<Measurement> measurements = await roomService.GetMeasurementsByRoomNameAsync(roomName);
                return Ok(measurements);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get measurements: {e}", e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{roomName}/measurements")]
        public async Task<ActionResult> AddMeasurements([FromRoute] string roomName,
            [FromBody]
            PostMeasurmentsDTO measurements)
        {
            try
            {
                _logger.LogInformation(
                    $"Received POST Request for /measurements: {JsonSerializer.Serialize(measurements)}");
                await roomService.AddMeasurements(measurements.DeviceId, roomName, measurements.Measurements);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to create measurements: {e}", e);
                return StatusCode(500, e.Message);
            }
        }
    }
}