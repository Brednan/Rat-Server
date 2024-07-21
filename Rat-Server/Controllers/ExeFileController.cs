using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Context;
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
    }
}
