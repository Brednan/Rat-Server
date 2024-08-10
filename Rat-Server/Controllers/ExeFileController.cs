using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
using Rat_Server.Model.Services;
using System.Linq.Expressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Rat_Server.Controllers
{
    [Authorize(Policy = "DeviceAuthenticated")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExeFileController : ControllerBase
    {
        private readonly RatDbContext _context;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;

        public ExeFileController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _jwtService = new JwtService(_config);
        }

        [HttpGet("GetExeFile/{FileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExeFile>> GetExeFile(string FileName)
        {
            ExeFile? exeFile = await _context.ExeFiles.SingleOrDefaultAsync(e => e.Name == FileName);
            if(exeFile == null)
            {
                return NotFound();
            }

            return File(exeFile.Content, "application/octet-stream", exeFile.Name);
        }
    }
}
