using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddSeoPropertiesToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanonicalUrl",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaKeywords",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Movies",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoFollow",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoIndex",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OgDescription",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgImage",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgTitle",
                table: "Movies",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Movies",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterCard",
                table: "Movies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Slug",
                table: "Movies",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_Slug",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CanonicalUrl",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MetaKeywords",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "NoFollow",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "NoIndex",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "OgDescription",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "OgImage",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "OgTitle",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "TwitterCard",
                table: "Movies");
        }
    }
}
