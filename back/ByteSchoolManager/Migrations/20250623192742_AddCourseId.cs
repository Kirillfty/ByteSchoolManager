using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_LessonId",
                table: "StudentCourses");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "StudentCourses",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_LessonId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_CourseId",
                table: "StudentCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_CourseId",
                table: "StudentCourses");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "StudentCourses",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_LessonId",
                table: "StudentCourses",
                column: "LessonId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
