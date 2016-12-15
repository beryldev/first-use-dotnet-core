using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentLines_ProductId",
                table: "DeliveryDocumentLines",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDocumentLines_Products_ProductId",
                table: "DeliveryDocumentLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDocumentLines_Products_ProductId",
                table: "DeliveryDocumentLines");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryDocumentLines_ProductId",
                table: "DeliveryDocumentLines");
        }
    }
}
