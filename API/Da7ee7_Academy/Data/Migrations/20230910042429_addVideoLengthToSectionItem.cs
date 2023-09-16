using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class addVideoLengthToSectionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoLength",
                table: "SectionItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoLength",
                table: "SectionItems");
        }
    }
}
