using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameOfCoach = table.Column<string>(type: "text", nullable: false),
                    NumberOfCoach = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameOfStudent = table.Column<string>(type: "text", nullable: false),
                    SurNameOfStudent = table.Column<string>(type: "text", nullable: false),
                    NameOfParent = table.Column<string>(type: "text", nullable: false),
                    SurNameOfParent = table.Column<string>(type: "text", nullable: false),
                    NumberOfStudent = table.Column<int>(type: "integer", nullable: false),
                    NumberOfParent = table.Column<int>(type: "integer", nullable: false),
                    CoachId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimeTableLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeekend = table.Column<string>(type: "text", nullable: false),
                    TimeOfLesson = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOfStartKurs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOfEndKurs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Curs = table.Column<string>(type: "text", nullable: false)
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
                    TimeTableLessonId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWorkedLesson = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoachId = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Students_CoachId",
                table: "Students",
                column: "CoachId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTimeTableLessons");

            migrationBuilder.DropTable(
                name: "StudentWorkedLessons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "WorkedLessons");

            migrationBuilder.DropTable(
                name: "TimeTableLessons");

            migrationBuilder.DropTable(
                name: "Coaches");
        }
    }
}
