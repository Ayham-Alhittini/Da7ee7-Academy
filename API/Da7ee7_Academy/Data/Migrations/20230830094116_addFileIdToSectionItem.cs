using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class addFileIdToSectionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "SectionItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectionItems_FileId",
                table: "SectionItems",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SectionItems_Files_FileId",
                table: "SectionItems",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SectionItems_Files_FileId",
                table: "SectionItems");

            migrationBuilder.DropIndex(
                name: "IX_SectionItems_FileId",
                table: "SectionItems");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "SectionItems");
        }
    }
}
