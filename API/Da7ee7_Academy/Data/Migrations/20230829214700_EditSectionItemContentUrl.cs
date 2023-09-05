using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class EditSectionItemContentUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "SectionItems");

            migrationBuilder.RenameColumn(
                name: "LectureUrl",
                table: "SectionItems",
                newName: "ContentUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentUrl",
                table: "SectionItems",
                newName: "LectureUrl");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "SectionItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
