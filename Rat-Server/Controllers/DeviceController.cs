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

        [HttpPost]
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

            // Create a Device object that we'll use to add to the database
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
    }
}
