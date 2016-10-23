using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    public partial class ReleaseDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReleaseDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    FullNumber = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDocumentLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    EAN = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    ReleaseDocumentId = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDocumentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseDocumentLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReleaseDocumentLines_ReleaseDocuments_ReleaseDocumentId",
                        column: x => x.ReleaseDocumentId,
                        principalTable: "ReleaseDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDocumentLines_ProductId",
                table: "ReleaseDocumentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDocumentLines_ReleaseDocumentId",
                table: "ReleaseDocumentLines",
                column: "ReleaseDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReleaseDocumentLines");

            migrationBuilder.DropTable(
                name: "ReleaseDocuments");
        }
    }
}
