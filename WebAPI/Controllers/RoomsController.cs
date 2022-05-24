using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementByRoomId([FromRoute] int roomId)
        {
            try
            {
                var measurements = await roomService.GetMeasurementsByRoomIdAsync(roomId);
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

        [HttpPut("{roomName}/devices/{deviceId}")]
        public async Task<ActionResult> UpdateRoomDevices([FromRoute] string roomName, [FromRoute] string deviceId)
        {
            try
            {
                await roomService.UpdateRoomDevicesAsync(roomName, deviceId);
                return Ok();

            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }


        [HttpGet("{roomName}/measurements")]
        public async Task<ActionResult<IEnumerable<ReadDeviceMeasurementsDTO>>> GetMeasurementByRoomName(
            [FromRoute]
            string roomName, [FromQuery] long? validFrom, [FromQuery] long? validTo)
        {
            try
            {
                IEnumerable<Measurement?> measurements = null;
                IList<ReadDeviceMeasurementsDTO> DTOList = new List<ReadDeviceMeasurementsDTO>();
                if (validFrom != null || validTo != null)
                {
                    var deviceMeasurementMap =
                        await roomService.GetRoomMeasurementsBetweenAsync(validFrom, validTo, roomName);

                    foreach (var key in deviceMeasurementMap.Keys)
                    {
                        var DTOToReturn = new ReadDeviceMeasurementsDTO()
                        {
                            DeviceId = key,
                            Measurements = deviceMeasurementMap[key]
                        };

                        DTOList.Add(DTOToReturn);
                    }
                }
                else
                {
                    var room = await roomService.GetRoomByNameAsync(roomName);


                    foreach (var roomClimateDevice in room.ClimateDevices)
                    {
                        var DTOToReturn = new ReadDeviceMeasurementsDTO()
                        {
                            DeviceId = roomClimateDevice.ClimateDeviceId,
                            Measurements = roomClimateDevice.Measurements
                        };
                        DTOList.Add(DTOToReturn);
                    }
                }

                return Ok(DTOList);
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(e.Message);
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
                await roomService.AddMeasurementsAsync(measurements.DeviceId, roomName, measurements.Measurements);
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