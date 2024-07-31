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
            var resultContent = GetObjectResultValue<List<Device>>(result);

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
                commandPlaceholders.Add(await CreateCommandPlaceholder(devicePlaceholder, $"Test Command {i}"));
            }

            var actionResult = await _controller.GetDeviceCommands(devicePlaceholder.Hwid);
            Assert.NotNull(actionResult.Result);
            Assert.IsType<OkObjectResult>(actionResult.Result);

            var responseValue = GetObjectResultValue<List<Command>>(actionResult.Result);
            Assert.IsType<List<Command>>(responseValue);
            Assert.Equal(responseValue.Count, commandPlaceholders.Count);

            for (int i = 0; i < responseValue.Count; i++)
            {
                Command? c = commandPlaceholders.Find(c => c.commandId.Equals(responseValue[i].commandId));
                Assert.NotNull(c);
                Assert.Equal(responseValue[i].DevicedHwid, c.DevicedHwid);
                Assert.Equal(responseValue[i].CommandValue, c.CommandValue);
                Assert.Equal(responseValue[i].DateAdded, c.DateAdded);
            }

            foreach(Command c in commandPlaceholders)
            {
                _context.Commands.Remove(c);
            }

            _context.Devices.Remove(devicePlaceholder);
            await _context.SaveChangesAsync();
        }
    }
}