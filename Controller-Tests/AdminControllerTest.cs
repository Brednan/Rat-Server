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

            Assert.NotNull(result.Value);
            Assert.IsType<OkObjectResult>(result.Result);

            Assert.True(result.Value.Count == infectedDevices.Count);
            for(int i = 0; i < infectedDevices.Count; i++)
            {
                Assert.Equal(result.Value[i], infectedDevices[i]);
            }
        }

        [Fact]
        public void TestGetDeviceCommands()
        {
            // Test with random Guid that isn't registered
            var result = _controller.GetDeviceCommands(new Guid()).Result;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}