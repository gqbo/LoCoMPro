using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class Create_Record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    NameRecord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    NameGenerator = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NameStore = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => new { x.NameRecord, x.RecordDate });
                    table.ForeignKey(
                        name: "FK_Record_GeneratorUser_NameGenerator",
                        column: x => x.NameGenerator,
                        principalTable: "GeneratorUser",
                        principalColumn: "UserName");
                    table.ForeignKey(
                        name: "FK_Record_Store_NameStore",
                        column: x => x.NameStore,
                        principalTable: "Store",
                        principalColumn: "NameStore");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Record_NameGenerator",
                table: "Record",
                column: "NameGenerator");

            migrationBuilder.CreateIndex(
                name: "IX_Record_NameStore",
                table: "Record",
                column: "NameStore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Record");
        }
    }
}
