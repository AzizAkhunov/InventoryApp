using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAccessKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryAccesses",
                table: "InventoryAccesses");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAccesses_InventoryId_UserId",
                table: "InventoryAccesses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryAccesses",
                table: "InventoryAccesses",
                columns: new[] { "InventoryId", "UserId" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 9, 47, 34, 193, DateTimeKind.Utc).AddTicks(3970));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 9, 47, 34, 193, DateTimeKind.Utc).AddTicks(3973));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 9, 47, 34, 193, DateTimeKind.Utc).AddTicks(3975));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 9, 47, 34, 193, DateTimeKind.Utc).AddTicks(3976));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsAdmin", "IsBlocked", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@test.com", true, false, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryAccesses",
                table: "InventoryAccesses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryAccesses",
                table: "InventoryAccesses",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 24, 7, 41, 51, 744, DateTimeKind.Utc).AddTicks(379));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 24, 7, 41, 51, 744, DateTimeKind.Utc).AddTicks(389));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 24, 7, 41, 51, 744, DateTimeKind.Utc).AddTicks(391));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 24, 7, 41, 51, 744, DateTimeKind.Utc).AddTicks(392));

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccesses_InventoryId_UserId",
                table: "InventoryAccesses",
                columns: new[] { "InventoryId", "UserId" },
                unique: true);
        }
    }
}
