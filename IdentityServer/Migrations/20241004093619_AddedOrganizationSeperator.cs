using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Migrations
{
    public partial class AddedOrganizationSeperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");
        }
    }
}
