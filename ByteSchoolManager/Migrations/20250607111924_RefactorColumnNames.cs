using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class RefactorColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_TimeTableLessonId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "NameOfParent",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "NameOfStudent",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "NumberOfCoach",
                table: "Coaches");

            migrationBuilder.RenameColumn(
                name: "SurNameOfStudent",
                table: "Students",
                newName: "ParentName");

            migrationBuilder.RenameColumn(
                name: "SurNameOfParent",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NumberOfStudent",
                table: "Students",
                newName: "StudentPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "NumberOfParent",
                table: "Students",
                newName: "ParentPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "TimeTableLessonId",
                table: "Lessons",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_TimeTableLessonId",
                table: "Lessons",
                newName: "IX_Lessons_CourseId");

            migrationBuilder.RenameColumn(
                name: "NameOfCoach",
                table: "Coaches",
                newName: "Telegram");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Coaches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Coaches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Coaches");

            migrationBuilder.RenameColumn(
                name: "StudentPhoneNumber",
                table: "Students",
                newName: "NumberOfStudent");

            migrationBuilder.RenameColumn(
                name: "ParentPhoneNumber",
                table: "Students",
                newName: "NumberOfParent");

            migrationBuilder.RenameColumn(
                name: "ParentName",
                table: "Students",
                newName: "SurNameOfStudent");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "SurNameOfParent");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Lessons",
                newName: "TimeTableLessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons",
                newName: "IX_Lessons_TimeTableLessonId");

            migrationBuilder.RenameColumn(
                name: "Telegram",
                table: "Coaches",
                newName: "NameOfCoach");

            migrationBuilder.AddColumn<string>(
                name: "NameOfParent",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameOfStudent",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCoach",
                table: "Coaches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_TimeTableLessonId",
                table: "Lessons",
                column: "TimeTableLessonId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
