using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWorkedLesson",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "TimeOfStartCourse",
                table: "Courses",
                newName: "DateOfStartCourse");

            migrationBuilder.RenameColumn(
                name: "TimeOfEndCourse",
                table: "Courses",
                newName: "DateOfEndCourse");

            migrationBuilder.RenameColumn(
                name: "DayOfWeekend",
                table: "Courses",
                newName: "DaysOfWeek");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAndTime",
                table: "Lessons",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAndTime",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "DaysOfWeek",
                table: "Courses",
                newName: "DayOfWeekend");

            migrationBuilder.RenameColumn(
                name: "DateOfStartCourse",
                table: "Courses",
                newName: "TimeOfStartCourse");

            migrationBuilder.RenameColumn(
                name: "DateOfEndCourse",
                table: "Courses",
                newName: "TimeOfEndCourse");

            migrationBuilder.AddColumn<DateTime>(
                name: "DayOfWorkedLesson",
                table: "Lessons",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
