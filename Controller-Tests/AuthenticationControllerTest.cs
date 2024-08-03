using Rat_Server.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Rat_Server.Controllers;
using Rat_Server.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Rat_Server.Model.DTOs;

namespace Controller_Tests
{
    public class AuthenticationControllerTest : BaseControllerTest
    {
        AuthenticationController _controller;

        public AuthenticationControllerTest(ITestOutputHelper output) : base(output)
        {
            _controller = new AuthenticationController(_context, _config);
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

        [Fact]
        private async void TestAdminLogin()
        {
            // Test Login on a user that doesn't exist
            ActionResult<JwtTokenDto> result = await _controller.AdminLogin(new UserLoginRequestBodyDto
            {

            });

            User userPlaceholder = await CreateUserPlaceholder("UserPlaceholder", "Password123");
            
            // Test 
        }
    }
}
