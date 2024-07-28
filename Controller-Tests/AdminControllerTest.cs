using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rat_Server.Controllers;
using Rat_Server.Model;
using Rat_Server.Model.Context;
using System.Text;

namespace Controller_Tests
{
    public class AdminControllerTest
    {
        private readonly RatDbContext _context;

        public AdminControllerTest()
        {
            var builder = WebApplication.CreateBuilder();
            
            DbContextOptionsBuilder<RatDbContext> options = new DbContextOptionsBuilder<RatDbContext>()
                .UseMySQL($"server={builder.Configuration["DATABASE_IP"]};" +
                          $"database={builder.Configuration["DATABASE_NAME"]};" +
                          $"user={builder.Configuration["DATABASE_USER"]};" +
                          $"password={builder.Configuration["DATABASE_PASSWORD"]}");

            _context = new RatDbContext(options.Options);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}