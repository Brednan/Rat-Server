using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Rat_Server.Model.Entities;

namespace Rat_Server.Model.Context
{
    public class RatDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User> Admin { get; set; }

        public RatDbContext(DbContextOptions<RatDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>();
            modelBuilder.Entity<Command>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Admin>();
        }
    }
}
