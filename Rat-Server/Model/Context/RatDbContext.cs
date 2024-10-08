﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Rat_Server.Model.Entities;

namespace Rat_Server.Model.Context
{
    public class RatDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ShellCode> ShellCodes { get; set; }
        public DbSet<ExeFile> ExeFiles { get; set; }

        public RatDbContext(DbContextOptions<RatDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().Property(d => d.LastActive).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Command>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Admin>();
            modelBuilder.Entity<ShellCode>();
            modelBuilder.Entity<ExeFile>();
        }
    }
}
