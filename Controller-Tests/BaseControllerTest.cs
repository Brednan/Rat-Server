using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Rat_Server.Controllers;
using Rat_Server.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller_Tests
{
    public class BaseControllerTest
    {
        protected readonly RatDbContext _context;

        public BaseControllerTest()
        {
            var builder = WebApplication.CreateBuilder();

            DbContextOptionsBuilder<RatDbContext> options = new DbContextOptionsBuilder<RatDbContext>()
                .UseMySQL($"server={builder.Configuration["DATABASE_IP"]};" +
                          $"database={builder.Configuration["DATABASE_NAME"]};" +
                          $"user={builder.Configuration["DATABASE_USER"]};" +
                          $"password={builder.Configuration["DATABASE_PASSWORD"]}");

            _context = new RatDbContext(options.Options);


        }
    }
}
