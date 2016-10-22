using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    public partial class DeliveryDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    FullNumber = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    DeliveryDocumentId = table.Column<int>(nullable: true),
                    EAN = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentLine_DeliveryDocuments_DeliveryDocumentId",
                        column: x => x.DeliveryDocumentId,
                        principalTable: "DeliveryDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentLine_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLine_DeliveryDocumentId",
                table: "DocumentLine",
                column: "DeliveryDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLine_ProductId",
                table: "DocumentLine",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentLine");

            migrationBuilder.DropTable(
                name: "DeliveryDocuments");
        }
    }
}
