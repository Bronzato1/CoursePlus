using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class AddQuizTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    ThumbnailId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_Thumbnails_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Thumbnails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "UpdatedTime" },
                values: new object[] { new DateTime(2020, 5, 25, 13, 43, 35, 501, DateTimeKind.Local).AddTicks(2694), new DateTime(2020, 5, 25, 13, 43, 35, 501, DateTimeKind.Local).AddTicks(3153) });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "UpdatedTime" },
                values: new object[] { new DateTime(2020, 5, 25, 13, 43, 35, 497, DateTimeKind.Local).AddTicks(5622), new DateTime(2020, 5, 25, 13, 43, 35, 500, DateTimeKind.Local).AddTicks(2333) });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CategoryId",
                table: "Quizzes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ImageId",
                table: "Quizzes",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ThumbnailId",
                table: "Quizzes",
                column: "ThumbnailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "UpdatedTime" },
                values: new object[] { new DateTime(2020, 5, 24, 14, 56, 47, 196, DateTimeKind.Local).AddTicks(1757), new DateTime(2020, 5, 24, 14, 56, 47, 196, DateTimeKind.Local).AddTicks(3068) });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "UpdatedTime" },
                values: new object[] { new DateTime(2020, 5, 24, 14, 56, 47, 187, DateTimeKind.Local).AddTicks(6545), new DateTime(2020, 5, 24, 14, 56, 47, 193, DateTimeKind.Local).AddTicks(8300) });
        }
    }
}
