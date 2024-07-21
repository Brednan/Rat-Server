using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Rat_Server.Migrations
{
    /// <inheritdoc />
    public partial class CreatedExeFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2024, 7, 21, 12, 22, 9, 127, DateTimeKind.Local).AddTicks(7177));

            migrationBuilder.CreateTable(
                name: "ExeFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExeFiles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExeFiles");

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
                oldDefaultValue: new DateTime(2024, 7, 21, 13, 59, 27, 195, DateTimeKind.Local).AddTicks(9884));
        }
    }
}
