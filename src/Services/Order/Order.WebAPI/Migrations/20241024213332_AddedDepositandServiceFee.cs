using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedDepositandServiceFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceFee",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ServiceFee",
                table: "Orders");
        }
    }
}
