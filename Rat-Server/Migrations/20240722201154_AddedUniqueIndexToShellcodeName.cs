using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueIndexToShellcodeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ShellCodes",
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
                defaultValue: new DateTime(2024, 7, 22, 13, 11, 54, 49, DateTimeKind.Local).AddTicks(6398),
                oldClrType: typeof(DateTime),
                oldType: "datetime(5)",
                oldPrecision: 5,
                oldDefaultValue: new DateTime(2024, 7, 22, 13, 10, 5, 0, DateTimeKind.Local).AddTicks(3242));

            migrationBuilder.CreateIndex(
                name: "IX_ShellCodes_Name",
                table: "ShellCodes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShellCodes_Name",
                table: "ShellCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ShellCodes",
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
                defaultValue: new DateTime(2024, 7, 22, 13, 10, 5, 0, DateTimeKind.Local).AddTicks(3242),
                oldClrType: typeof(DateTime),
                oldType: "datetime(5)",
                oldPrecision: 5,
                oldDefaultValue: new DateTime(2024, 7, 22, 13, 11, 54, 49, DateTimeKind.Local).AddTicks(6398));
        }
    }
}
