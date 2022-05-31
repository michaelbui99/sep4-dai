using System;
using System.Collections;
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
        private readonly IDeviceService _deviceService;
        private ILogger _logger;

        public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }


        /// <summary>
        /// Method to get all the devices from the service and expose them as an end point
        /// through a http get request.
        /// </summary>
        /// <exception>if anything goes wrong return status code 500</exception>
        /// <returns>if ok return a list with all the devices</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllDevicesDTO>>> GetAllDevices()
        {
            try
            {
                var roomNameMapping = await _deviceService.GetRoomNamesForDevices();
                var devicesToReturn = new List<GetAllDevicesDTO>();

                foreach (var climateDevice in await _deviceService.GetAllDevicesExcludingMeasurements())
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


        /// <summary>
        /// Method to get all the settings for one device from the service and expose them
        /// as an end point through a http get request.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <exception>if the device id it does not have valid input return bad request exception</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <returns>if ok return all the settings for a device</returns>
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