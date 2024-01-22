using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mairala202.Migrations
{
    public partial class MemberUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Members",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Members",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Members",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Members",
                newName: "Description");
        }
    }
}
