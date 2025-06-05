using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTimeTableLessons");

            migrationBuilder.DropTable(
                name: "StudentWorkedLessons");

            migrationBuilder.DropTable(
                name: "WorkedLessons");

            migrationBuilder.DropTable(
                name: "TimeTableLessons");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DayOfWeekend = table.Column<int>(type: "integer", nullable: false),
                    TimeOfLesson = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    TimeOfStartCourse = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeOfEndCourse = table.Column<DateOnly>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CoachId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimeTableLessonId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWorkedLesson = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoachId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Courses_TimeTableLessonId",
                        column: x => x.TimeTableLessonId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CoachId",
                table: "Courses",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CoachId",
                table: "Lessons",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TimeTableLessonId",
                table: "Lessons",
                column: "TimeTableLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_LessonId",
                table: "StudentCourses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLessons_LessonId",
                table: "StudentLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLessons_StudentId",
                table: "StudentLessons",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "StudentLessons");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.CreateTable(
                name: "TimeTableLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    Curs = table.Column<string>(type: "text", nullable: false),
                    DayOfWeekend = table.Column<string>(type: "text", nullable: false),
                    TimeOfEndKurs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOfLesson = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOfStartKurs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTableLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeTableLessons_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentTimeTableLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    TimetableLessonsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTimeTableLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTimeTableLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTimeTableLessons_TimeTableLessons_TimetableLessonsId",
                        column: x => x.TimetableLessonsId,
                        principalTable: "TimeTableLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkedLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    TimeTableLessonId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWorkedLesson = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkedLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkedLessons_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkedLessons_TimeTableLessons_TimeTableLessonId",
                        column: x => x.TimeTableLessonId,
                        principalTable: "TimeTableLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentWorkedLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    WorkedLessonId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWorkedLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentWorkedLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWorkedLessons_WorkedLessons_WorkedLessonId",
                        column: x => x.WorkedLessonId,
                        principalTable: "WorkedLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTimeTableLessons_StudentId",
                table: "StudentTimeTableLessons",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTimeTableLessons_TimetableLessonsId",
                table: "StudentTimeTableLessons",
                column: "TimetableLessonsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWorkedLessons_StudentId",
                table: "StudentWorkedLessons",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWorkedLessons_WorkedLessonId",
                table: "StudentWorkedLessons",
                column: "WorkedLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableLessons_CoachId",
                table: "TimeTableLessons",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkedLessons_CoachId",
                table: "WorkedLessons",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkedLessons_TimeTableLessonId",
                table: "WorkedLessons",
                column: "TimeTableLessonId");
        }
    }
}
