using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class RenamedDeviceHwid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Devices_DeviceHwid",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_DeviceHwid",
                table: "Commands");

            migrationBuilder.RenameColumn(
                name: "Hwid",
                table: "Devices",
                newName: "DeviceHwid");

            migrationBuilder.RenameColumn(
                name: "DeviceName",
                table: "Devices",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_Hwid_DeviceName",
                table: "Devices",
                newName: "IX_Devices_DeviceHwid_Name");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceHwid1",
                table: "Commands",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Commands_DeviceHwid1",
                table: "Commands",
                column: "DeviceHwid1");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Devices_DeviceHwid1",
                table: "Commands",
                column: "DeviceHwid1",
                principalTable: "Devices",
                principalColumn: "DeviceHwid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Devices_DeviceHwid1",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_DeviceHwid1",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "DeviceHwid1",
                table: "Commands");

            migrationBuilder.RenameColumn(
                name: "DeviceHwid",
                table: "Devices",
                newName: "Hwid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Devices",
                newName: "DeviceName");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_DeviceHwid_Name",
                table: "Devices",
                newName: "IX_Devices_Hwid_DeviceName");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_DeviceHwid",
                table: "Commands",
                column: "DeviceHwid");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Devices_DeviceHwid",
                table: "Commands",
                column: "DeviceHwid",
                principalTable: "Devices",
                principalColumn: "Hwid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
