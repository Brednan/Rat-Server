using Microsoft.EntityFrameworkCore;
using Rat_Server.Model.Context;
using Rat_Server.Model.Services;
using Rat_Server.Model.Entities;
using System.Globalization;

namespace Rat_Server.Custom_Middleware
{
    public class UpdateDeviceLastActive: IMiddleware
    {
        private readonly JwtService _jwtService;
        private readonly RatDbContext _dbContext;

        public UpdateDeviceLastActive(IConfiguration config, RatDbContext dbContext)
        {
            _jwtService = new JwtService(config);
            _dbContext = dbContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var authHeader = context.Request.Headers.Authorization;

            /* If an authentication header was supplied, check if its for a Device and if so,
            update its last active data to the current time */
            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                try
                {
                    string jwt = _jwtService.ParseAuthorizationHeader(authHeader);
                    string? hwid = _jwtService.GetJwtClaimValue(jwt, "Hwid");
                    if (hwid != null)
                    {
                        Device? device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Hwid.Equals(new Guid(hwid)));
                        if (device != null)
                        {
                            device.LastActive = DateTime.Now;
                            _dbContext.Devices.Update(device);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
                catch { }
            }

            await next(context);
        }
    }

    public static class UpdateDeviceLastActiveExtension
    {
        public static IApplicationBuilder UseFactoryActivatedMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UpdateDeviceLastActive>();
        }
    }
}
