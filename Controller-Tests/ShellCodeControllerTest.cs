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
using Microsoft.AspNetCore.Mvc;
using Rat_Server.Model.DTOs;

namespace Controller_Tests
{
    public class ShellCodeControllerTest: BaseControllerTest
    {
        ShellCodeController _controller;

        public ShellCodeControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new ShellCodeController(_context, _config);
        }

        [Fact]
        private async Task TestGetShellCode()
        {
            ShellCode shellCodePlaceholder = await CreateShellCodePlaceholder(Guid.NewGuid().ToString(), "FFFFFFFFFF");
            
            var result = await _controller.GetShellCode(shellCodePlaceholder.Name);
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            ShellCodeDto shellCodeDto = GetObjectResultValue<ShellCodeDto>(result);

            Assert.Equal(shellCodePlaceholder.Code, shellCodeDto.Code);
            Assert.Equal(shellCodePlaceholder.Name, shellCodeDto.Name);

            await DeleteShellCodePlaceholder(shellCodePlaceholder);
        }
    }
}
