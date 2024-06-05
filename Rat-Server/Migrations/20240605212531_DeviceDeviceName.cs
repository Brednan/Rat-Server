using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class DeviceDeviceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Devices",
                newName: "DeviceName");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_Hwid_Username",
                table: "Devices",
                newName: "IX_Devices_Hwid_DeviceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeviceName",
                table: "Devices",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_Hwid_DeviceName",
                table: "Devices",
                newName: "IX_Devices_Hwid_Username");
        }
    }
}
