using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class AddEnrolledOnStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Enrolled",
                table: "Students",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enrolled",
                table: "Students");
        }
    }
}
