using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    /// <inheritdoc />
    public partial class Modified_Record_Store : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Canton_NameProvince_NameCanton",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Canton_Province_NameProvince",
                table: "Canton");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Store_NameStore",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Store_Canton_NameProvince_NameCanton",
                table: "Store");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Store",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Record_NameStore",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Canton",
                table: "Canton");

            migrationBuilder.RenameTable(
                name: "Canton",
                newName: "Cantons");

            migrationBuilder.AlterColumn<string>(
                name: "NameProvince",
                table: "Store",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameCanton",
                table: "Store",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCanton",
                table: "Record",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameProvince",
                table: "Record",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Store",
                table: "Store",
                columns: new[] { "NameStore", "NameProvince", "NameCanton" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cantons",
                table: "Cantons",
                columns: new[] { "NameProvince", "NameCanton" });

            migrationBuilder.CreateIndex(
                name: "IX_Record_NameStore_NameProvince_NameCanton",
                table: "Record",
                columns: new[] { "NameStore", "NameProvince", "NameCanton" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cantons_NameProvince_NameCanton",
                table: "AspNetUsers",
                columns: new[] { "NameProvince", "NameCanton" },
                principalTable: "Cantons",
                principalColumns: new[] { "NameProvince", "NameCanton" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cantons_Province_NameProvince",
                table: "Cantons",
                column: "NameProvince",
                principalTable: "Province",
                principalColumn: "NameProvince",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Store_NameStore_NameProvince_NameCanton",
                table: "Record",
                columns: new[] { "NameStore", "NameProvince", "NameCanton" },
                principalTable: "Store",
                principalColumns: new[] { "NameStore", "NameProvince", "NameCanton" });

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Cantons_NameProvince_NameCanton",
                table: "Store",
                columns: new[] { "NameProvince", "NameCanton" },
                principalTable: "Cantons",
                principalColumns: new[] { "NameProvince", "NameCanton" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cantons_NameProvince_NameCanton",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cantons_Province_NameProvince",
                table: "Cantons");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Store_NameStore_NameProvince_NameCanton",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Store_Cantons_NameProvince_NameCanton",
                table: "Store");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Store",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Record_NameStore_NameProvince_NameCanton",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cantons",
                table: "Cantons");

            migrationBuilder.DropColumn(
                name: "NameCanton",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "NameProvince",
                table: "Record");

            migrationBuilder.RenameTable(
                name: "Cantons",
                newName: "Canton");

            migrationBuilder.AlterColumn<string>(
                name: "NameCanton",
                table: "Store",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameProvince",
                table: "Store",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Store",
                table: "Store",
                column: "NameStore");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Canton",
                table: "Canton",
                columns: new[] { "NameProvince", "NameCanton" });

            migrationBuilder.CreateIndex(
                name: "IX_Record_NameStore",
                table: "Record",
                column: "NameStore");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Canton_NameProvince_NameCanton",
                table: "AspNetUsers",
                columns: new[] { "NameProvince", "NameCanton" },
                principalTable: "Canton",
                principalColumns: new[] { "NameProvince", "NameCanton" });

            migrationBuilder.AddForeignKey(
                name: "FK_Canton_Province_NameProvince",
                table: "Canton",
                column: "NameProvince",
                principalTable: "Province",
                principalColumn: "NameProvince",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Store_NameStore",
                table: "Record",
                column: "NameStore",
                principalTable: "Store",
                principalColumn: "NameStore");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Canton_NameProvince_NameCanton",
                table: "Store",
                columns: new[] { "NameProvince", "NameCanton" },
                principalTable: "Canton",
                principalColumns: new[] { "NameProvince", "NameCanton" });
        }
    }
}
