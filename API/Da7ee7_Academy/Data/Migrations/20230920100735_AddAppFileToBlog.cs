using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddAppFileToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppFileId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AppFileId",
                table: "Blogs",
                column: "AppFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Files_AppFileId",
                table: "Blogs",
                column: "AppFileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Files_AppFileId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AppFileId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "AppFileId",
                table: "Blogs");
        }
    }
}
