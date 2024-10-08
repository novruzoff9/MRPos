using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrasturucture.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompanyIdToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Products");
        }
    }
}
