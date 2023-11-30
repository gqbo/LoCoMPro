using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class ListTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    NameList = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => new { x.NameList, x.UserName });
                    table.ForeignKey(
                        name: "FK_Lists_GeneratorUser_UserName",
                        column: x => x.UserName,
                        principalTable: "GeneratorUser",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listed",
                columns: table => new
                {
                    NameList = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameProduct = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listed", x => new { x.NameList, x.UserName, x.NameProduct });
                    table.ForeignKey(
                        name: "FK_Listed_Lists_NameList_UserName",
                        columns: x => new { x.NameList, x.UserName },
                        principalTable: "Lists",
                        principalColumns: new[] { "NameList", "UserName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listed_Product_NameProduct",
                        column: x => x.NameProduct,
                        principalTable: "Product",
                        principalColumn: "NameProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listed_NameProduct",
                table: "Listed",
                column: "NameProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_UserName",
                table: "Lists",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Listed");

            migrationBuilder.DropTable(
                name: "Lists");
        }
    }
}
