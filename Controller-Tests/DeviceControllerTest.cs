using Microsoft.EntityFrameworkCore;
using Rat_Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Xunit.Abstractions;
using Rat_Server.Model.Entities;
using Rat_Server.Model.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Controller_Tests
{
    public class DeviceControllerTest : BaseControllerTest
    {
        DeviceController _controller;

        public DeviceControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new DeviceController(_context, _config);
        }

        [Fact]
        private async Task GetCurrentCommandTest()
        {
            Device devicePlaceholder = await CreateDevicePlaceholder(Guid.NewGuid(), Guid.NewGuid().ToString());
            List<Command> commandPlaceholders = new List<Command>();

            string authToken = $"Bearer { _jwtService.GenerateJwtToken([new Claim("DeviceRegistered", "true"),
                                                                        new Claim("Hwid", devicePlaceholder.Hwid.ToString()),
                                                                        new Claim(ClaimTypes.Name, devicePlaceholder.Name)], false) }";

            // Add 10 commands to the Device placeholder
            for (int i = 0; i < 10; i++)
            {
                Command commandPlaceholder = await CreateCommandPlaceholder(devicePlaceholder, "Command Test" + i);
                commandPlaceholders.Add(commandPlaceholder);
            }

            var result = await _controller.GetCurrentCommand(authToken);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);

            DeviceCommandDto commandResult = GetObjectResultValue<DeviceCommandDto>(result);
            Assert.Equal(new Guid(commandResult.commandId), commandPlaceholders[0].commandId);
            Assert.Equal(commandResult.CommandValue, commandPlaceholders[0].CommandValue);

            foreach(Command command in commandPlaceholders)
            {
                await DeleteCommandPlaceholder(command);
            }

            await DeleteDevicePlaceholder(devicePlaceholder);
        }
    }
}
