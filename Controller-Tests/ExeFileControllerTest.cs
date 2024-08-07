using Microsoft.EntityFrameworkCore;
using Rat_Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Security.Claims;
using Xunit.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using Rat_Server.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Controller_Tests
{
    public class ExeFileControllerTest : BaseControllerTest
    {
        ExeFileController _controller;

        public ExeFileControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new ExeFileController(_context, _config);
        }

        [Fact]
        private async Task TestGetExeFile()
        {
            ExeFile exePlaceholder = await CreateExeFilePlaceholder();

            var result = await _controller.GetExeFile(exePlaceholder.Name);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);

            ExeFile exeFileResult = GetObjectResultValue<ExeFile>(result);
            Assert.Equal(exeFileResult.Name, exePlaceholder.Name);
            Assert.Equal(exeFileResult.Content, exePlaceholder.Content);
            Assert.Equal(exeFileResult.Id, exePlaceholder.Id);

            await DeleteExeFilePlaceholder(exePlaceholder);
        }
    }
}
