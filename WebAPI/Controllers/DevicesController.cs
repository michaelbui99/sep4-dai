using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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


        [HttpGet("{deviceId}/settings")]
        public async Task<ActionResult<GetDeviceSettingDTO>> GetDeviceSettings(string deviceId)
        {
            try
            {
                var settings = await _deviceService.GetDeviceByIdAsync(deviceId);
                var settingsToReturn = new GetDeviceSettingDTO()
                {
                    Co2Threshold = settings.Settings.Co2Threshold,
                    HumidityThreshold = settings.Settings.HumidityThreshold,
                    TargetTemperature = settings.Settings.TargetTemperature,
                    TemperatureMargin = settings.Settings.TemperatureMargin
                };
                return Ok(settingsToReturn);

            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}