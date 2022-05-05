using System.Collections;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Repositories;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomService roomService;

        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
    
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurmentByRoomId([FromQuery] int RoomId) {
            try
            {
                IEnumerable<Measurement> measurements = await roomService.GetMeasurementsByRoomIdAsync(RoomId);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
    
    

}