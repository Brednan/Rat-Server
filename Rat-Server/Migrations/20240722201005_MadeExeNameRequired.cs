using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class MadeExeNameRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExeFiles",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActive",
                table: "Devices",
                type: "datetime(5)",
                precision: 5,
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 13, 10, 5, 0, DateTimeKind.Local).AddTicks(3242),
                oldClrType: typeof(DateTime),
                oldType: "datetime(5)",
                oldPrecision: 5,
                oldDefaultValue: new DateTime(2024, 7, 21, 13, 59, 27, 195, DateTimeKind.Local).AddTicks(9884));

            migrationBuilder.CreateIndex(
                name: "IX_ExeFiles_Name",
                table: "ExeFiles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExeFiles_Name",
                table: "ExeFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExeFiles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastActive",
                table: "Devices",
                type: "datetime(5)",
                precision: 5,
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 13, 59, 27, 195, DateTimeKind.Local).AddTicks(9884),
                oldClrType: typeof(DateTime),
                oldType: "datetime(5)",
                oldPrecision: 5,
                oldDefaultValue: new DateTime(2024, 7, 22, 13, 10, 5, 0, DateTimeKind.Local).AddTicks(3242));
        }
    }
}
