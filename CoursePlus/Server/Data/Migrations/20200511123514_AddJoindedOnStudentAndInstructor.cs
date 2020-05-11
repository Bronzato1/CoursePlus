using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class AddJoindedOnStudentAndInstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Joinded",
                table: "Students",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Joinded",
                table: "Instructors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Joinded",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Joinded",
                table: "Instructors");
        }
    }
}
