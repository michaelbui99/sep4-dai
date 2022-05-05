using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
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
    
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurmentByRoomId([FromRoute] int roomId) {
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
    }
    
    

}