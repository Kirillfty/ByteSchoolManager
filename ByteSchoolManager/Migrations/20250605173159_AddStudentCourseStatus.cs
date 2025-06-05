using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentCourseStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentCourses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentCourses");
        }
    }
}
