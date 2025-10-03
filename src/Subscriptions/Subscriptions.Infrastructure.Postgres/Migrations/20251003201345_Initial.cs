using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Subscriptions.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.CreateTable(
                name: "kits",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    avatar_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ipk_kits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kit_items",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    kit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("kit_items_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_kit_items_kits_kit_id",
                        column: x => x.kit_id,
                        principalSchema: "users",
                        principalTable: "kits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_kit_items_products_product_id",
                        column: x => x.product_id,
                        principalSchema: "products",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kits_details",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sum = table.Column<int>(type: "integer", nullable: false),
                    KitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("kit_details_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_kits_details_kits_KitId",
                        column: x => x.KitId,
                        principalSchema: "users",
                        principalTable: "kits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    KitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ipk_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscriptions_kits_KitId",
                        column: x => x.KitId,
                        principalSchema: "users",
                        principalTable: "kits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kit_items_kit_id",
                schema: "users",
                table: "kit_items",
                column: "kit_id");

            migrationBuilder.CreateIndex(
                name: "IX_kit_items_product_id",
                schema: "users",
                table: "kit_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_kits_details_KitId",
                schema: "users",
                table: "kits_details",
                column: "KitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_KitId",
                schema: "users",
                table: "subscriptions",
                column: "KitId");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_user_id",
                schema: "users",
                table: "subscriptions",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kit_items",
                schema: "users");

            migrationBuilder.DropTable(
                name: "kits_details",
                schema: "users");

            migrationBuilder.DropTable(
                name: "subscriptions",
                schema: "users");

            migrationBuilder.DropTable(
                name: "kits",
                schema: "users");
        }
    }
}
