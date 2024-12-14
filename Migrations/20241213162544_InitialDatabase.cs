using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tenant_service.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    api_tenant_id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    domain = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    note = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(type: "TINYINT(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenants", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "tenants",
                columns: new[] { "id", "api_tenant_id", "created_at", "domain", "note", "tenant_id", "updated_at" },
                values: new object[,]
                {
                    { 1, "dcDxl607BQHvvdoH0AkqfajnZEhm78uB", new DateTime(2024, 12, 13, 23, 25, 43, 833, DateTimeKind.Local).AddTicks(6200), "shoes-store", "This is an example tenant", "49ef20c8-5b7d-44f8-bd15-1f7cc225e692", new DateTime(2024, 12, 13, 23, 25, 43, 833, DateTimeKind.Local).AddTicks(6230) },
                    { 2, "gdW9acf9PpVtUoWKgxRv6VPdbuaPEMdV", new DateTime(2024, 12, 13, 23, 25, 43, 833, DateTimeKind.Local).AddTicks(6310), "shuba-grocery", "This is an example tenant", "2a7bcc6f-4baf-4e6a-95b6-846ddddb4919", new DateTime(2024, 12, 13, 23, 25, 43, 833, DateTimeKind.Local).AddTicks(6310) },
                    { 3, "lZXUrIcS3AwsddpeQFoJvzurBX7Pb30o", new DateTime(2024, 12, 13, 23, 25, 43, 833, DateTimeKind.Local).AddTicks(6320), "max-toys-store", "This is an example tenant", "8d34c62c-a23b-4902-9644-550b686b36dc", new DateTime(2024, 12, 13, 23, 25, 43, 833, DateTimeKind.Local).AddTicks(6320) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenants");
        }
    }
}
