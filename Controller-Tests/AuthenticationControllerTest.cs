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

namespace Controller_Tests
{
    public class AuthenticationControllerTest : BaseControllerTest
    {
        AuthenticationController _controller;
        JwtService _jwtService;

        public AuthenticationControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new AuthenticationController(_context, _config);
            _jwtService = new JwtService(_config);
        }

        private async Task<User> CreateUserPlaceholder(string name, string password)
        {
            AuthenticationService authenticationService = new AuthenticationService();
            string hashedPassword = authenticationService.HashPassword(password);

            User userPlaceholder = new User
            {
                Name = name,
                Password = hashedPassword,
                UserId = Guid.NewGuid()
            };

            await _context.Users.AddAsync(userPlaceholder);
            await _context.SaveChangesAsync();

            return userPlaceholder;
        }

        private async Task<Admin> CreateAdminPlaceholder(User userPlaceholder)
        {
            Admin adminPlaceholder = new Admin
            {
                User = userPlaceholder
            };

            await _context.Admins.AddAsync(adminPlaceholder);
            await _context.SaveChangesAsync();
            
            return adminPlaceholder;
        }

        private async Task<Admin> CreateAdminPlaceholder(string name, string password)
        {
            User userPlaceholder = await CreateUserPlaceholder(name, password);
            return await CreateAdminPlaceholder(userPlaceholder);
        }

        private async Task DeleteAdminPlaceholder(Admin adminPlaceholder)
        {
            _context.Admins.Remove(adminPlaceholder);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteUserPlaceholder(User userPlaceholder)
        {
            _context.Users.Remove(userPlaceholder);
            await _context.SaveChangesAsync();
        }

        [Fact]
        private async void TestAdminLogin()
        {
            UserLoginRequestBodyDto loginRequestBodyDto = new UserLoginRequestBodyDto
            {
                Username = "UserPlaceholder",
                Password = "Password123"
            };

            // Test Login on a user that doesn't exist
            ActionResult<JwtTokenDto> result = await _controller.AdminLogin(loginRequestBodyDto);

            Assert.IsType<UnauthorizedResult>(result.Result);

            // Test with a user that exists but is not an admin
            User userPlaceholder = await CreateUserPlaceholder("UserPlaceholder", "Password123");

            result = await _controller.AdminLogin(loginRequestBodyDto);

            Assert.IsType<UnauthorizedResult>(result.Result);

            // Test with a user that is an admin
            Admin adminPlaceholder = await CreateAdminPlaceholder(userPlaceholder);

            result = await _controller.AdminLogin(loginRequestBodyDto);

            Assert.IsType<OkObjectResult>(result.Result);
            
            JwtTokenDto jwtToken = GetObjectResultValue<JwtTokenDto>(result);

            Assert.NotNull(jwtToken.Token);
            Assert.NotEmpty(jwtToken.Token);
            _output.WriteLine(_jwtService.DecodeJwtString(jwtToken.Token).Claims.First().Value);
            Assert.Equal("UserPlaceholder", _jwtService.GetJwtClaimValue(jwtToken.Token, JwtRegisteredClaimNames.Name));

            // Cleanup
            await DeleteAdminPlaceholder(adminPlaceholder);
            await DeleteUserPlaceholder(userPlaceholder);
        }
    }
}
