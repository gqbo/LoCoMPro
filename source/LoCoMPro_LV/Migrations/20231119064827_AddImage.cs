using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class AddImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    NameGenerator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => new { x.NameGenerator, x.RecordDate, x.NameImage });
                    table.ForeignKey(
                        name: "FK_Images_Record_NameGenerator_RecordDate",
                        columns: x => new { x.NameGenerator, x.RecordDate },
                        principalTable: "Record",
                        principalColumns: new[] { "NameGenerator", "RecordDate" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
