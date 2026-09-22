using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddFAQAndAboutUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 500, nullable: false),
                    CustomerCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ActiveUserCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalVideoCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SubscriberCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AwardCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AboutUsId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsComments_AboutUs_AboutUsId",
                        column: x => x.AboutUsId,
                        principalTable: "AboutUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsTeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AboutUsId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsTeamMembers_AboutUs_AboutUsId",
                        column: x => x.AboutUsId,
                        principalTable: "AboutUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsSocialLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsSocialLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsSocialLinks_AboutUsTeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "AboutUsTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsComments_AboutUsId_IsApproved",
                table: "AboutUsComments",
                columns: new[] { "AboutUsId", "IsApproved" });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSocialLinks_TeamMemberId_Order",
                table: "AboutUsSocialLinks",
                columns: new[] { "TeamMemberId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsTeamMembers_AboutUsId_Order",
                table: "AboutUsTeamMembers",
                columns: new[] { "AboutUsId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_Order",
                table: "FAQs",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_CreatedAt",
                table: "UserQuestions",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsComments");

            migrationBuilder.DropTable(
                name: "AboutUsSocialLinks");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "UserQuestions");

            migrationBuilder.DropTable(
                name: "AboutUsTeamMembers");

            migrationBuilder.DropTable(
                name: "AboutUs");
        }
    }
}
