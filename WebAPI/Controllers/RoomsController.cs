using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
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

        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
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
                Console.WriteLine($"Received: {JsonSerializer.Serialize(measurements)}");
                await roomService.AddMeasurements(measurements.DeviceId, roomName, measurements.Measurements);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}