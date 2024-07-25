using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rat_Server.Model.Context;
using Rat_Server.Model.DTOs;
using Rat_Server.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Rat_Server.Controllers
{
    /// <summary>
    /// This controller handles everything related to authentication.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    { 
        private readonly RatDbContext _context;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<string> _passwordHasher;

        public AuthenticationController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _passwordHasher = new PasswordHasher<string>();
        }
        
        private string GenerateJwtToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_KEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["JWT_ISSUER"],
              _config["JWT_ISSUER"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return verificationResult == PasswordVerificationResult.Success;
        }

        [HttpPost("AdminLogin")]
        [ProducesResponseType(typeof(JwtTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<JwtTokenDto> AdminLogin([FromBody] UserLoginRequestBodyDto requestBody)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            User? userInfo = _context.Users.FirstOrDefault(u => u.Name == requestBody.Username);

            if (userInfo == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            if(!VerifyPassword(userInfo.Password, requestBody.Password))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            Admin? admin = _context.Admins.FirstOrDefault(a => a.UserId == userInfo.UserId);

            return Ok(new JwtTokenDto
            {
                Token = admin != null ? GenerateJwtToken([new Claim(JwtRegisteredClaimNames.NameId, userInfo.UserId.ToString()),
                                                            new Claim(JwtRegisteredClaimNames.Name, userInfo.Name),
                                                            new Claim("Admin", "true")]) 
                                          : GenerateJwtToken([new Claim(JwtRegisteredClaimNames.NameId, userInfo.UserId.ToString()),
                                                            new Claim(JwtRegisteredClaimNames.Name, userInfo.Name)])
            });
        }

        [HttpPost("RegisterDevice")]
        [ProducesResponseType<JwtTokenDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<JwtTokenDto>> RegisterDevice([FromBody] RegisterDeviceRequestBodyDto deviceInfo)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(deviceInfo);
            }

            if (await _context.Devices.SingleOrDefaultAsync<Device>(d => d.Hwid.Equals(deviceInfo.Hwid)) != null)
            {
                return Conflict("A device with the same Hwid already exists");
            }

            Device newDevice = new Device
            {
                Hwid = new Guid(deviceInfo.Hwid),
                Name = deviceInfo.DeviceName
            };

            _context.Devices.Add(newDevice);
            _context.SaveChanges();

            return Ok(newDevice);
        }
    }
}
