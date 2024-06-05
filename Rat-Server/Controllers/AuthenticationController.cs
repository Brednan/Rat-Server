using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rat_Server.Model;
using Rat_Server.Model.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Rat_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    { 
        private readonly RatDbContext _context;
        private readonly IConfiguration _config;

        private string generateJwtToken(JwtSecurityToken jwtToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        public AuthenticationController(RatDbContext context, IConfiguration config) 
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> RegisterDevice([FromBody] RegisterDeviceRequestDto requestBody)
        {
            // Check if request body is valid
            if(!ModelState.IsValid)
            {
                return BadRequest(requestBody);
            }

            // Check if the device is already registered
            if(_context.Devices.Single(d => d.Hwid.ToString() == requestBody.Hwid) != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            return Ok("");  // Placeholder
        }
    }
}
