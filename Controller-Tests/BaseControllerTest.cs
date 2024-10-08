﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
using Rat_Server.Model.Services;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Controller_Tests
{
    public class BaseControllerTest
    {
        protected readonly RatDbContext _context;
        protected readonly IConfiguration _config;
        protected readonly ITestOutputHelper _output;
        protected readonly JwtService _jwtService;
        protected readonly AuthenticationService _authenticationService;

        public BaseControllerTest(ITestOutputHelper output)
        {
            _output = output;

            var config = new ConfigurationBuilder().AddUserSecrets<BaseControllerTest>().Build();

            DbContextOptionsBuilder<RatDbContext> options = new DbContextOptionsBuilder<RatDbContext>()
                .UseInMemoryDatabase(databaseName: "RatControllerTest");

            _config = config;

            _context = new RatDbContext(options.Options);
            _jwtService = new JwtService(_config);
            _authenticationService = new AuthenticationService();
        }


        /// <summary>
        /// Creates a device object with a random HWID, adds it
        /// to the database and returns the result. This is meant for
        /// a test that requires a random device to be in the database.
        /// </summary>
        /// <param name="Hwid">The Hwid of the Device</param>
        /// <param name="Name">The name of the Device</param>
        /// <returns>The newly created Device</returns>
        protected async Task<Device> CreateDevicePlaceholder(Guid Hwid, string Name)
        {
            Device device = new Device
            {
                Hwid = Hwid,
                Name = "TestGetDeviceCommands",
                LastActive = DateTime.Now
            };

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
            return device;
        }

        protected async Task<Command> CreateCommandPlaceholder(Device device, string CommandValue)
        {
            Command command = new Command
            {
                commandId = Guid.NewGuid(),
                DevicedHwid = device.Hwid,
                CommandValue = CommandValue,
                Device = device,
                DateAdded = DateTime.Now
            };
            await _context.Commands.AddAsync(command);
            await _context.SaveChangesAsync();

            return command;
        }

        protected async Task<ShellCode> CreateShellCodePlaceholder(string shellCodeName, string shellCodeValue)
        {
            ShellCode shellCode = new ShellCode
            {
                Code = shellCodeValue,
                Name = shellCodeName
            };

            await _context.ShellCodes.AddAsync(shellCode);
            await _context.SaveChangesAsync();

            return shellCode;
        }

        protected async Task<User> CreateUserPlaceholder(string name, string password)
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

        protected async Task<Admin> CreateAdminPlaceholder(User userPlaceholder)
        {
            Admin adminPlaceholder = new Admin
            {
                User = userPlaceholder
            };

            await _context.Admins.AddAsync(adminPlaceholder);
            await _context.SaveChangesAsync();

            return adminPlaceholder;
        }

        protected async Task<Admin> CreateAdminPlaceholder(string name, string password)
        {
            User userPlaceholder = await CreateUserPlaceholder(name, password);
            return await CreateAdminPlaceholder(userPlaceholder);
        }

        protected async Task<ExeFile> CreateExeFilePlaceholder()
        {
            ExeFile exeFile = new ExeFile
            {
               
                Name = Guid.NewGuid().ToString(), // Use Guid for the name to make sure it's unique
                Content = new byte[1024]
            };

            await _context.ExeFiles.AddAsync(exeFile);
            await _context.SaveChangesAsync();

            return await _context.ExeFiles.SingleAsync(e => e.Name.Equals(exeFile.Name));
        }

        protected async Task DeleteCommandPlaceholder(Command commandPlaceholder)
        {
            _context.Commands.Remove(commandPlaceholder);
            await _context.SaveChangesAsync();
        }

        protected async Task DeleteExeFilePlaceholder(ExeFile exePlaceholder)
        {
            _context.ExeFiles.Remove(exePlaceholder);
            await _context.SaveChangesAsync();
        }

        protected async Task DeleteAdminPlaceholder(Admin adminPlaceholder)
        {
            _context.Admins.Remove(adminPlaceholder);
            await _context.SaveChangesAsync();
        }

        protected async Task DeleteUserPlaceholder(User userPlaceholder)
        {
            _context.Users.Remove(userPlaceholder);
            await _context.SaveChangesAsync();
        }

        protected async Task DeleteDevicePlaceholder(Device devicePlaceholder)
        {
            _context.Devices.Remove(devicePlaceholder);
            await _context.SaveChangesAsync();
        }

        protected async Task DeleteShellCodePlaceholder(ShellCode shellCodePlaceholder)
        {
            _context.ShellCodes.Remove(shellCodePlaceholder);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Used for parsing the Value contained in an ActionResult.Result object.
        /// This is meant for parsing the content returned from a Controller function.
        /// </summary>
        /// <typeparam name="T">The object Type to be retrieved from the result</typeparam>
        /// <param name="result">The ActionResult returned from the Controller function.</param>
        /// <returns></returns>
        protected static T GetObjectResultValue<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}
