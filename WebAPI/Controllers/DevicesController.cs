using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DevicesController : ControllerBase
    {
        private IDeviceService _deviceService;
        private ILogger _logger;

        public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllDevicesDTO>>> GetAllDevices()
        {
            try
            {
                var roomNameMapping = await _deviceService.GetRoomNamesForDevices();
                var devicesToReturn = new List<GetAllDevicesDTO>();
                foreach (var climateDevice in await _deviceService.GetAllDevicesAsync())
                {
                    var deviceDTO = new GetAllDevicesDTO()
                    {
                        ClimateDeviceId = climateDevice.ClimateDeviceId,
                        Settings = climateDevice.Settings,
                        RoomName = roomNameMapping[climateDevice.ClimateDeviceId]
                    };
                    devicesToReturn.Add(deviceDTO);
                }
                return Ok(devicesToReturn);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}