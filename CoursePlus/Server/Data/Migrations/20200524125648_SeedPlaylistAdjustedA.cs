using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class SeedPlaylistAdjustedA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "UpdatedTime" },
                values: new object[] { new DateTime(2020, 5, 24, 14, 50, 18, 871, DateTimeKind.Local).AddTicks(2309), new DateTime(2020, 5, 24, 14, 50, 18, 871, DateTimeKind.Local).AddTicks(3130) });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "UpdatedTime" },
                values: new object[] { new DateTime(2020, 5, 24, 14, 50, 18, 863, DateTimeKind.Local).AddTicks(6013), new DateTime(2020, 5, 24, 14, 50, 18, 869, DateTimeKind.Local).AddTicks(3553) });
        }
    }
}
