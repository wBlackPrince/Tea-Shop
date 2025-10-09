using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tea_Shop.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddGistIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_departments_path",
                table: "comments",
                column: "path")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_departments_path",
                table: "comments");
        }
    }
}
