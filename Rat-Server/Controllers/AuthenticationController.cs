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
using Rat_Server.Model.Services;


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
        private readonly JwtService _jwtService;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(RatDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _jwtService = new JwtService(_config);
            _authenticationService = new AuthenticationService();
        }

        [HttpPost("AdminLogin")]
        [ProducesResponseType(typeof(JwtTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<JwtTokenDto>> AdminLogin([FromBody] UserLoginRequestBodyDto requestBody)
        {
            User? userInfo = await _context.Users.FirstOrDefaultAsync(u => u.Name == requestBody.Username);

            if (userInfo == null || !_authenticationService.VerifyPassword(userInfo.Password, requestBody.Password))
            {
                return Unauthorized();
            }

            Admin? admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userInfo.UserId);
            if (admin == null)
            {
                return Unauthorized();
            }

            return Ok(new JwtTokenDto
            {
                Token = _jwtService.GenerateJwtToken([new Claim(JwtRegisteredClaimNames.NameId, userInfo.UserId.ToString()),
                                                      new Claim(JwtRegisteredClaimNames.Name, userInfo.Name),
                                                      new Claim("Admin", "true")], true)
            });
        }

        [HttpPost("RegisterDevice")]
        [ProducesResponseType<Device>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Device>> RegisterDevice([FromBody] RegisterDeviceRequestBodyDto deviceInfo)
        {
            if (await _context.Devices.SingleOrDefaultAsync(d => d.Hwid.Equals(deviceInfo.Hwid)) != null)
            {
                return Conflict("A device with the same Hwid already exists");
            }

            Device newDevice = new Device
            {
                Hwid = deviceInfo.Hwid,
                Name = deviceInfo.DeviceName
            };

            _context.Devices.Add(newDevice);
            _context.SaveChanges();

            return Ok(newDevice);
        }

        [HttpPost("DeviceLogin")]
        [ProducesResponseType<JwtTokenDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<JwtTokenDto>> DeviceLogin([FromBody] Guid Hwid)
        {
            Device? device = await _context.Devices.SingleOrDefaultAsync(d => d.Hwid.Equals(Hwid));
            if (device == null)
            {
                return Unauthorized();
            }

            return Ok(new JwtTokenDto
            {
                Token = _jwtService.GenerateJwtToken([new Claim("DeviceRegistered", "true"),
                                                      new Claim("Hwid", device.Hwid.ToString()),
                                                      new Claim(ClaimTypes.Name, device.Name)], false)
            });
        }
    }
}
