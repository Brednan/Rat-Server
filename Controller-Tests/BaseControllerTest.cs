﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rat_Server.Model.Context;
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

            _output.WriteLine("Config Test Output: " + config["DATABASE_IP"]);

            DbContextOptionsBuilder<RatDbContext> options = new DbContextOptionsBuilder<RatDbContext>()
                .UseMySQL($"server={config["DATABASE_IP"]};" +
                          $"database={config["DATABASE_NAME"]};" +
                          $"user={config["DATABASE_USER"]};" +
                          $"password={config["DATABASE_PASSWORD"]}");

            _context = new RatDbContext(options.Options);
            _config = config;
        }
    }
}
