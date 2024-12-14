using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tenant_service.Migrations.Tenants
{
    /// <inheritdoc />
    public partial class InitialTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(type: "TINYINT(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 10, 22, 54, 58, 473, DateTimeKind.Local).AddTicks(6490), "Uzumaki Naruto", new DateTime(2024, 12, 10, 22, 54, 58, 473, DateTimeKind.Local).AddTicks(6490) },
                    { 2, new DateTime(2024, 12, 10, 22, 54, 58, 473, DateTimeKind.Local).AddTicks(6490), "Hayate Kakashi", new DateTime(2024, 12, 10, 22, 54, 58, 473, DateTimeKind.Local).AddTicks(6500) },
                    { 3, new DateTime(2024, 12, 10, 22, 54, 58, 473, DateTimeKind.Local).AddTicks(6500), "Uchiha Madara", new DateTime(2024, 12, 10, 22, 54, 58, 473, DateTimeKind.Local).AddTicks(6500) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
