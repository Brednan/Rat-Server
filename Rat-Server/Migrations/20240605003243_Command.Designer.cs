﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rat_Server.Model;

#nullable disable

namespace Rat_Server.Migrations
{
    [DbContext(typeof(RatDbContext))]
    [Migration("20240605003243_Command")]
    partial class Command
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Rat_Server.Model.Command", b =>
                {
                    b.Property<Guid>("commandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CommandValue")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<DateTime>("DateAdded")
                        .HasPrecision(5)
                        .HasColumnType("datetime2(5)");

                    b.Property<Guid>("DeviceHwid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("commandId");

                    b.HasIndex("DeviceHwid");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("Rat_Server.Model.Device", b =>
                {
                    b.Property<Guid>("Hwid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastActive")
                        .HasPrecision(5)
                        .HasColumnType("datetime2(5)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Hwid");

                    b.HasIndex("Hwid", "Username");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Rat_Server.Model.Command", b =>
                {
                    b.HasOne("Rat_Server.Model.Device", "Device")
                        .WithMany("Commands")
                        .HasForeignKey("DeviceHwid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("Rat_Server.Model.Device", b =>
                {
                    b.Navigation("Commands");
                });
#pragma warning restore 612, 618
        }
    }
}
