using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rat_Server.Model.Context;

namespace Rat_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShellCodeController : ControllerBase
    {
        private readonly RatDbContext _context;
        private readonly IConfiguration _config;
     
        public ShellCodeController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


    }
}
