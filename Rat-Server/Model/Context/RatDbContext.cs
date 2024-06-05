using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Rat_Server.Model
{
    public class RatDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Command> Commands { get; set; }

        public RatDbContext(DbContextOptions<RatDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>();
            modelBuilder.Entity<Command>();
        }
    }
}
