using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernorateToSalePoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "SalePoints",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "SalePoints");
        }
    }
}
