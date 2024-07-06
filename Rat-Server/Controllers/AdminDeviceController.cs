using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rat_Server.Model.Context;
using Rat_Server.Model.DTOs;

namespace Rat_Server.Controllers
{
    /// <summary>
    /// This controller contains endpoints that perform 
    /// administrative actions on the infected devices.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDeviceController : ControllerBase
    {
        private readonly RatDbContext _context;

        public AdminDeviceController(RatDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllInfectedDevices")]
        public ActionResult<List<DeviceCommandDto>> GetAllInfectedDevices()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
