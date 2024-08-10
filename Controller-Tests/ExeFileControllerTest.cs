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
            Assert.NotNull(result);
            Assert.IsType<FileContentResult>(result);

            // Use a variable with FileContentResult as the defined data type rather than using 'var'
            FileContentResult exeFileResult = (FileContentResult) result;

            Assert.Equal(exeFileResult.FileContents, exePlaceholder.Content);
            Assert.Equal(exeFileResult.FileDownloadName, exePlaceholder.Name);

            await DeleteExeFilePlaceholder(exePlaceholder);
        }
    }
}
