using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class addFileIdToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FileId",
                table: "Courses",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Files_FileId",
                table: "Courses",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Files_FileId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_FileId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Courses");
        }
    }
}
