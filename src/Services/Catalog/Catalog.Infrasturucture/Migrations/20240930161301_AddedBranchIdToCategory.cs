using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrasturucture.Migrations
{
    /// <inheritdoc />
    public partial class AddedBranchIdToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Categories",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Categories");
        }
    }
}
