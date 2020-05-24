using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class SeedPlaylistAdjusted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "Id", "CategoryId", "CreatedTime", "CreatedUser", "Description", "Difficulty", "Featured", "ImageId", "Language", "OwnerId", "Popular", "Price", "SubTitle", "ThumbnailId", "Title", "UpdatedTime", "UpdatedUser" },
                values: new object[] { 1, 28, new DateTime(2020, 5, 24, 14, 50, 18, 871, DateTimeKind.Local).AddTicks(2309), "azur.consult@gmail.com", "My description", 0, false, null, 0, 1, false, 0, "My subtitle", null, "My title", new DateTime(2020, 5, 24, 14, 50, 18, 871, DateTimeKind.Local).AddTicks(3130), "azur.consult@gmail.com" });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "CreatedUser", "UpdatedTime", "UpdatedUser" },
                values: new object[] { new DateTime(2020, 5, 24, 14, 50, 18, 863, DateTimeKind.Local).AddTicks(6013), "azur.consult@gmail.com", new DateTime(2020, 5, 24, 14, 50, 18, 869, DateTimeKind.Local).AddTicks(3553), "azur.consult@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "CreatedUser", "UpdatedTime", "UpdatedUser" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null });
        }
    }
}
