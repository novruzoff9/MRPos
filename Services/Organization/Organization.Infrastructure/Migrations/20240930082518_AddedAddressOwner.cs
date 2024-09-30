using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoogleMapsLocation",
                table: "Branches",
                newName: "Address_GoogleMapsLocation");

            migrationBuilder.AlterColumn<string>(
                name: "Address_GoogleMapsLocation",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_GoogleMapsEmbed",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Region",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Address_GoogleMapsEmbed",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Address_Region",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "Address_GoogleMapsLocation",
                table: "Branches",
                newName: "GoogleMapsLocation");

            migrationBuilder.AlterColumn<string>(
                name: "GoogleMapsLocation",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
