﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rat_Server.Model.Context;
using Rat_Server.Model.Entities;
using Xunit.Abstractions;

namespace Controller_Tests
{
    public class BaseControllerTest
    {
        protected readonly RatDbContext _context;
        protected readonly IConfiguration _config;
        protected readonly ITestOutputHelper _output;

        public BaseControllerTest(ITestOutputHelper output)
        {
            _output = output;

            var config = new ConfigurationBuilder().AddUserSecrets<BaseControllerTest>().Build();

            DbContextOptionsBuilder<RatDbContext> options = new DbContextOptionsBuilder<RatDbContext>()
                .UseMySQL($"server={config["DATABASE_IP"]};" +
                          $"database={config["DATABASE_NAME"]};" +
                          $"user={config["DATABASE_USER"]};" +
                          $"password={config["DATABASE_PASSWORD"]}");

            _context = new RatDbContext(options.Options);
            _config = config;
        }


        /// <summary>
        /// Created a device object with a random HWID, adds it
        /// to the database and returns the result. This is meant for
        /// a test that requires a random device to be in the database.
        /// </summary>
        /// <param name="Name">The name of the Device</param>
        /// <returns>The newly created Device</returns>
        protected async Task<Device> CreateDevicePlaceholder(string Name)
        {
            Device device = new Device
            {
                Hwid = new Guid(),  // Use a random Hwid
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
                DevicedHwid = device.Hwid,
                CommandValue = CommandValue,
                Device = device
            };
            await _context.Commands.AddAsync(command);
            await _context.SaveChangesAsync();

            return command;
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
