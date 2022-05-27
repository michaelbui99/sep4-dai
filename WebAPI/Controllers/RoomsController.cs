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

        /// <summary>
        /// Method to get measurement for a room by the room id from the service
        /// exposing it through http get request
        /// </summary>
        /// <exception>if the id is not properly formatted returns not found</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <param name="roomId"></param>
        /// <returns>ok response containing a list of measurements from the specified room</returns>
   
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
        
        /// <summary>
        /// Method to update the room device by room name and device id from the
        /// service exposing it through http put request
        /// </summary>
        /// <exception>if the id or the room name is not properly formatted returns not found</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <param name="roomName"></param>
        /// <param name="deviceId"></param>
        /// <returns>ok meaning that the update went successfully</returns>
 
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
        
        /// <summary>
        /// Method for configuring the setting for each room using room name and settings
        /// to the service exposing it through http put request
        /// </summary>
        /// <exception>if the room name or the settings are not properly formatted returns not found</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <param name="roomName"></param>
        /// <param name="settings"></param>
        /// <returns>ok meaning that the settings were set</returns>

        [HttpPut("{roomName}/settings")]
        public async Task<ActionResult> SetSettings([FromRoute] string roomName, [FromBody] SetSettingsDTO settings)
        {
            try
            {
                var newSettings = new Settings()
                {
                    Co2Threshold = settings.Co2Threshold,
                    HumidityThreshold = settings.HumidityThreshold,
                    TargetTemperature = settings.TargetTemperature,
                    TemperatureMargin = settings.TemperatureMargin
                };
                await roomService.SetSettingsAsync(roomName, newSettings);
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
        
        /// <summary>
        /// Method to get the measurements from a room by using room name and time period
        /// from the service exposing it through http get request
        /// </summary>
        ///<exception>if there are no measurements for that particular room it returns bad request</exception>
        /// <exception>if the room name or the time period are not properly formatted returns not found</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <param name="roomName"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <returns>ok response a list with the device measurements</returns>

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


        /// <summary>
        /// Method to add measurements to a room using room name and measurement
        /// exposing it through a http post request
        /// </summary>
        /// <exception>if the room name or the measurement are not properly formatted returns not found</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <param name="roomName"></param>
        /// <param name="measurements"></param>
        /// <returns>ok response meaning that the measurements were added</returns>
        
        
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
        
        

        /// <summary>
        /// Method for getting all rooms through exposing a http get request
        /// </summary>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <returns>ok response with a list of rooms</returns>
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllRoomsDTO>>> GetAllRooms()
        {
            try
            {
               
                var allRoms = await roomService.GetAllRoomsAsync();
                var roomsToReturn = new List<GetAllRoomsDTO>();
                foreach (var room in allRoms)
                {
                    var newRoom = new GetAllRoomsDTO()
                    {
                        RoomName = room.RoomName,
                    };
                    roomsToReturn.Add(newRoom);
                }
                return Ok(roomsToReturn);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Method for adding a new room exposing through a http post request and reading the room object from the body.
        /// </summary>
        /// <exception>if the room object is not properly formatted returns bad request</exception>
        /// <exception>if anything went wrong return status code 500</exception>
        /// <param name="newRoom"></param>
        /// <returns>ok meaning there is a new room added</returns>
        
        [HttpPost]
        public async Task<ActionResult> AddNewRoom([FromBody] NewRoomDTO newRoom)
        {
            try
            {
                await roomService.CreateNewRoomAsync(newRoom.RoomName);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}