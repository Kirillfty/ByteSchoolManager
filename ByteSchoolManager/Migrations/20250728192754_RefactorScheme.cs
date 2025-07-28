using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class RefactorScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Coaches_CoachId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CoachId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Lessons");

            migrationBuilder.AddColumn<bool>(
                name: "Marked",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Moved",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Replaced",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marked",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Moved",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Replaced",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Students",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CoachId",
                table: "Students",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Coaches_CoachId",
                table: "Students",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }
    }
}
