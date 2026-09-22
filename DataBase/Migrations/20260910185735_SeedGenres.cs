using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class SeedGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "ArchivedAt", "CreatedAt", "DeletedAt", "IsActive", "IsArchived", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "اکشن", null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "کمدی", null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "درام", null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "ترسناک", null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "علمی تخیلی", null },
                    { 6, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "عاشقانه", null },
                    { 7, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "جنایی", null },
                    { 8, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "ماجراجویی", null },
                    { 9, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "فانتزی", null },
                    { 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "مستند", null },
                    { 11, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "تاریخی", null },
                    { 12, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "جنگی", null },
                    { 13, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "خانوادگی", null },
                    { 14, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, false, "انیمیشن", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
