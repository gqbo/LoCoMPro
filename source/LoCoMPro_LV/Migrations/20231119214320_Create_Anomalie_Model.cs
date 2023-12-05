using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class Create_Anomalie_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anomalies",
                columns: table => new
                {
                    NameGenerator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anomalies", x => new { x.NameGenerator, x.RecordDate, x.Type });
                    table.ForeignKey(
                        name: "FK_Anomalies_Record_NameGenerator_RecordDate",
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
                name: "Anomalies");
        }
    }
}
