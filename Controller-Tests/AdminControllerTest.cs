using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rat_Server.Controllers;
using Rat_Server.Model;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
using System.Text;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MySqlX.XDevAPI.Common;

namespace Controller_Tests
{
    public class AdminControllerTest: BaseControllerTest
    {
        protected readonly AdminController _controller;

        public AdminControllerTest(ITestOutputHelper output) : base(output) 
        {
            _controller = new AdminController(_context);
        }

        [Fact]
        public async void TestGetAllInfectedDevices()
        {
            List<Device> infectedDevices = await _context.Devices.ToListAsync();
            var result = await _controller.GetAllInfectedDevices();
            var resultContent = GetObjectResultContent<List<Device>>(result);

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(resultContent);
            Assert.True(resultContent.Count == infectedDevices.Count);
            
            for(int i = 0; i < infectedDevices.Count; i++)
            {
                Assert.Equal(resultContent[i], infectedDevices[i]);
            }
        }

        [Fact]
        public async void TestGetDeviceCommands()
        {
            Device devicePlaceholder = await CreateDevicePlaceholder("TestGetDeviceCommands");
            List<Command> commandPlaceholders = [];
            
            for (int i = 0; i < 10; i++)
            {
                commandPlaceholders.Add(await CreateCommandPlaceholder(devicePlaceholder.Hwid, $"Test Command {i}"));
            }
            

        }
    }
}