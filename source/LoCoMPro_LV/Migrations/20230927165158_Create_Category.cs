using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class Create_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    NameCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameTopCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.NameCategory);
                    table.ForeignKey(
                        name: "FK_Category_Category_NameTopCategory",
                        column: x => x.NameTopCategory,
                        principalTable: "Category",
                        principalColumn: "NameCategory");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_NameTopCategory",
                table: "Category",
                column: "NameTopCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
