using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddTeachedStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Students_Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Courses_TeacherId",
                table: "Students_Courses",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_Teachers_TeacherId",
                table: "Students_Courses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_Teachers_TeacherId",
                table: "Students_Courses");

            migrationBuilder.DropIndex(
                name: "IX_Students_Courses_TeacherId",
                table: "Students_Courses");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Students_Courses");
        }
    }
}
