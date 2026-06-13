using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCarSpecifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "HorsePower",
                table: "Cars",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Luggage",
                table: "Cars",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Seats",
                table: "Cars",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transmission",
                table: "Cars",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "HorsePower",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Luggage",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "Cars");
        }
    }
}
