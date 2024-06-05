using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class Device : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Hwid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime2(5)", precision: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Hwid);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    commandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceHwid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommandValue = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2(5)", precision: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.commandId);
                    table.ForeignKey(
                        name: "FK_Commands_Devices_DeviceHwid",
                        column: x => x.DeviceHwid,
                        principalTable: "Devices",
                        principalColumn: "Hwid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commands_DeviceHwid",
                table: "Commands",
                column: "DeviceHwid");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Hwid_Username",
                table: "Devices",
                columns: new[] { "Hwid", "Username" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
