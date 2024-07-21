using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedShellCodeDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ShellCodes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "longblob");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActive",
                table: "Devices",
                type: "datetime(5)",
                precision: 5,
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 12, 22, 9, 127, DateTimeKind.Local).AddTicks(7177),
                oldClrType: typeof(DateTime),
                oldType: "datetime(5)",
                oldPrecision: 5,
                oldDefaultValue: new DateTime(2024, 7, 8, 18, 49, 32, 34, DateTimeKind.Local).AddTicks(26));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Code",
                table: "ShellCodes",
                type: "longblob",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActive",
                table: "Devices",
                type: "datetime(5)",
                precision: 5,
                nullable: false,
                defaultValue: new DateTime(2024, 7, 8, 18, 49, 32, 34, DateTimeKind.Local).AddTicks(26),
                oldClrType: typeof(DateTime),
                oldType: "datetime(5)",
                oldPrecision: 5,
                oldDefaultValue: new DateTime(2024, 7, 21, 12, 22, 9, 127, DateTimeKind.Local).AddTicks(7177));
        }
    }
}
