using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class Ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "StudentLessons",
                newName: "LessonStatus");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "StudentCourses",
                newName: "CourseStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonStatus",
                table: "StudentLessons",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CourseStatus",
                table: "StudentCourses",
                newName: "Status");
        }
    }
}
