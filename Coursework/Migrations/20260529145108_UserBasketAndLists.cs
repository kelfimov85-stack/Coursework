using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursework.Migrations
{
    /// <inheritdoc />
    public partial class UserBasketAndLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasketProducts",
                columns: table => new
                {
                    BasketsId = table.Column<int>(type: "integer", nullable: false),
                    ProductsBasketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketProducts", x => new { x.BasketsId, x.ProductsBasketId });
                    table.ForeignKey(
                        name: "FK_BasketProducts_Basket_BasketsId",
                        column: x => x.BasketsId,
                        principalTable: "Basket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketProducts_Products_ProductsBasketId",
                        column: x => x.ProductsBasketId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_ProductsBasketId",
                table: "BasketProducts",
                column: "ProductsBasketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketProducts");
        }
    }
}
