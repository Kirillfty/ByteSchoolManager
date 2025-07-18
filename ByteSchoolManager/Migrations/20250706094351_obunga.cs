using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class obunga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Coaches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_UserId",
                table: "Coaches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Users_UserId",
                table: "Coaches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Users_UserId",
                table: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_UserId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Coaches");
        }
    }
}
