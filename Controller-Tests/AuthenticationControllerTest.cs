using Rat_Server.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Rat_Server.Controllers;
using Rat_Server.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Xunit.Abstractions;
using Rat_Server.Model.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Controller_Tests
{
    public class AuthenticationControllerTest : BaseControllerTest
    {
        AuthenticationController _controller;

        public AuthenticationControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new AuthenticationController(_context, _config);
        }

        [Fact]
        private async Task TestAdminLogin()
        {
            // Use a Guid to generate a value for the Username to make sure it's unique for this test
            string randomUsernameValue = Guid.NewGuid().ToString();
            _output.WriteLine(randomUsernameValue);

            UserLoginRequestBodyDto loginRequestBodyDto = new UserLoginRequestBodyDto
            {
                Username = randomUsernameValue,
                Password = "Password123"
            };

            // Test Login on a user that doesn't exist
            ActionResult<JwtTokenDto> result = await _controller.AdminLogin(loginRequestBodyDto);

            Assert.IsType<UnauthorizedResult>(result.Result);

            // Test with a user that exists but is not an admin
            User userPlaceholder = await CreateUserPlaceholder(randomUsernameValue, "Password123");

            result = await _controller.AdminLogin(loginRequestBodyDto);

            Assert.IsType<UnauthorizedResult>(result.Result);

            // Test with a user that is an admin
            Admin adminPlaceholder = await CreateAdminPlaceholder(userPlaceholder);

            result = await _controller.AdminLogin(loginRequestBodyDto);

            Assert.IsType<OkObjectResult>(result.Result);
            
            JwtTokenDto jwtToken = GetObjectResultValue<JwtTokenDto>(result);

            Assert.NotNull(jwtToken.Token);
            Assert.NotEmpty(jwtToken.Token);
            Assert.Equal(randomUsernameValue, _jwtService.GetJwtClaimValue(jwtToken.Token, JwtRegisteredClaimNames.Name));

            // Cleanup
            await DeleteAdminPlaceholder(adminPlaceholder);
            await DeleteUserPlaceholder(userPlaceholder);
        }

        [Fact]
        private async Task TestRegisterDevice()
        {
            // Generate a Guid for the Hwid to use for the placeholder device
            Guid Hwid = Guid.NewGuid();

            // Use a Guid to generate a value for the Name to make sure it's unique for this test
            string deviceName = Guid.NewGuid().ToString();

            // Create the Device Placeholder we'll use for the test
            Device devicePlaceholder = await CreateDevicePlaceholder(Hwid, deviceName);

            // Create the payload we'll use for testing the endpoint
            RegisterDeviceRequestBodyDto testPayload = new RegisterDeviceRequestBodyDto
            {
                Hwid = Hwid.ToString(),
                DeviceName = deviceName
            };

            // Test with the Hwid of a device that already exists
            var result = await _controller.RegisterDevice(testPayload);

            // Make sure we got a Conflict result since the device aalready exists
            Assert.IsType<ConflictObjectResult>(result.Result);

            // Delete the Device Placeholder
            await DeleteDevicePlaceholder(devicePlaceholder);

            // Try to register the device now that it doesn't exist anymore
            result = await _controller.RegisterDevice(testPayload);

            // Make sure we successfully added the device
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(GetObjectResultValue<Device>(result));

            Device? addedDevice = await _context.Devices.SingleOrDefaultAsync(d => d.Hwid.Equals(Hwid));

            // Make sure the Device is in the database
            Assert.NotNull(addedDevice);

            // Cleanup
            await DeleteDevicePlaceholder(addedDevice);
        }

        [Fact]
        private async Task TestDeviceLogin()
        {
            // Generate a Guid for the Hwid to use for the placeholder device
            Guid Hwid = Guid.NewGuid();

            // Create the Device Placeholder we'll use for the test
            Device devicePlaceholder = await CreateDevicePlaceholder(Hwid, Guid.NewGuid().ToString());

            // Test with the Hwid of a device that already exists
            var result = await _controller.DeviceLogin(Hwid.ToString());

            // Make sure we got a 200 response
            Assert.IsType<OkObjectResult>(result.Result);
            
            // Make sure the Token in the response isn't empty
            Assert.NotEmpty(GetObjectResultValue<JwtTokenDto>(result).Token);

            // Delete the Device Placeholder
            await DeleteDevicePlaceholder(devicePlaceholder);

            // Try to login with the placeholder now that it doesn't exist
            result = await _controller.DeviceLogin(Hwid.ToString());

            // Make sure we got an Unauthorized result
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}
