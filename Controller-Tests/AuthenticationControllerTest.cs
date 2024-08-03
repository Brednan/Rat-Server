using Microsoft.AspNetCore.Mvc;
using Rat_Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Controller_Tests
{
    public class AuthenticationControllerTest : BaseControllerTest
    {
        AuthenticationController _controller;

        public AuthenticationControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new AuthenticationController(_context, _config);
        }
    }
}
