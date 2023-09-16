using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Da7ee7_Academy.Migrations
{
    /// <inheritdoc />
    public partial class WatchedLecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchedLectures",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SectionItemId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    WatchedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedLectures", x => new { x.StudentId, x.SectionItemId });
                    table.ForeignKey(
                        name: "FK_WatchedLectures_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchedLectures_SectionItems_SectionItemId",
                        column: x => x.SectionItemId,
                        principalTable: "SectionItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WatchedLectures_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchedLectures_CourseId",
                table: "WatchedLectures",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedLectures_SectionItemId",
                table: "WatchedLectures",
                column: "SectionItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedLectures");
        }
    }
}
