using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class added_constraint_conversation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conversations_ProductId",
                table: "Conversations");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Conversations_ProductId_SenderId_ReceiverId",
                table: "Conversations",
                columns: new[] { "ProductId", "SenderId", "ReceiverId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Conversations_ProductId_SenderId_ReceiverId",
                table: "Conversations");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ProductId",
                table: "Conversations",
                column: "ProductId");
        }
    }
}
