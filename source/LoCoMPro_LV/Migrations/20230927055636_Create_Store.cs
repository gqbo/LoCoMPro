using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class Create_Store : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    NameStore = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameProvince = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NameCanton = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.NameStore);
                    table.ForeignKey(
                        name: "FK_Store_Canton_NameProvince_NameCanton",
                        columns: x => new { x.NameProvince, x.NameCanton },
                        principalTable: "Canton",
                        principalColumns: new[] { "NameProvince", "NameCanton" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Store_NameProvince_NameCanton",
                table: "Store",
                columns: new[] { "NameProvince", "NameCanton" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Store");
        }
    }
}
