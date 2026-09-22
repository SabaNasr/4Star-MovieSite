using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCastMemberToManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastMembers_Movies_MovieId",
                table: "CastMembers");

            migrationBuilder.DropIndex(
                name: "IX_CastMembers_MovieId",
                table: "CastMembers");

            migrationBuilder.DropColumn(
                name: "CastType",
                table: "CastMembers");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "CastMembers");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "CastMembers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MovieCastMembers",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    CastMemberId = table.Column<int>(type: "int", nullable: false),
                    CastType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCastMembers", x => new { x.MovieId, x.CastMemberId });
                    table.ForeignKey(
                        name: "FK_MovieCastMembers_CastMembers_CastMemberId",
                        column: x => x.CastMemberId,
                        principalTable: "CastMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCastMembers_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_FullName",
                table: "CastMembers",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCastMembers_CastMemberId",
                table: "MovieCastMembers",
                column: "CastMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieCastMembers");

            migrationBuilder.DropIndex(
                name: "IX_CastMembers_FullName",
                table: "CastMembers");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "CastMembers");

            migrationBuilder.AddColumn<string>(
                name: "CastType",
                table: "CastMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "CastMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_MovieId",
                table: "CastMembers",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_CastMembers_Movies_MovieId",
                table: "CastMembers",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
