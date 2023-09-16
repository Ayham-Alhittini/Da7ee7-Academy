using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class addCourseIdToSectionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "SectionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SectionItems_CourseId",
                table: "SectionItems",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionItems_Courses_CourseId",
                table: "SectionItems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionItems_Courses_CourseId",
                table: "SectionItems");

            migrationBuilder.DropIndex(
                name: "IX_SectionItems_CourseId",
                table: "SectionItems");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "SectionItems");
        }
    }
}
