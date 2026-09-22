using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class SeedLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "ArchivedAt", "CreatedAt", "DeletedAt", "IsActive", "IsArchived", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "فارسی", null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "انگلیسی", null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "فرانسوی", null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "آلمانی", null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "اسپانیایی", null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "ایتالیایی", null },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "ژاپنی", null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "کره‌ای", null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "چینی", null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "عربی", null },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "ترکی", null },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "روسی", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
