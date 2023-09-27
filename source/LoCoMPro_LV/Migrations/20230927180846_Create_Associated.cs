using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class Create_Associated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Associated",
                columns: table => new
                {
                    NameProduct = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associated", x => new { x.NameProduct, x.NameCategory });
                    table.ForeignKey(
                        name: "FK_Associated_Category_NameCategory",
                        column: x => x.NameCategory,
                        principalTable: "Category",
                        principalColumn: "NameCategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Associated_Product_NameProduct",
                        column: x => x.NameProduct,
                        principalTable: "Product",
                        principalColumn: "NameProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Associated_NameCategory",
                table: "Associated",
                column: "NameCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Associated");
        }
    }
}
