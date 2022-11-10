using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddUserFavoritesFIX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyProperty_Products_ProductId1",
                table: "MyProperty");

            migrationBuilder.DropIndex(
                name: "IX_MyProperty_ProductId1",
                table: "MyProperty");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "MyProperty");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "MyProperty",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_MyProperty_ProductId",
                table: "MyProperty",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyProperty_Products_ProductId",
                table: "MyProperty",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyProperty_Products_ProductId",
                table: "MyProperty");

            migrationBuilder.DropIndex(
                name: "IX_MyProperty_ProductId",
                table: "MyProperty");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MyProperty",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "MyProperty",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyProperty_ProductId1",
                table: "MyProperty",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MyProperty_Products_ProductId1",
                table: "MyProperty",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
