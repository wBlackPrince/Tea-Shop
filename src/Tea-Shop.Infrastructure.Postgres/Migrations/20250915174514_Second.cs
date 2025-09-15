using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tea_Shop.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rating",
                table: "products",
                newName: "sum_ratings");

            migrationBuilder.AddColumn<int>(
                name: "count_ratings",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count_ratings",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "sum_ratings",
                table: "products",
                newName: "rating");
        }
    }
}
