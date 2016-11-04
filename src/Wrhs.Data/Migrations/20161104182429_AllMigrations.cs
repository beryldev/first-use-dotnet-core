using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    public partial class AllMigrations : Migration
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
                    IssueDate = table.Column<DateTime>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    FullNumber = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelocationDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    FullNumber = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelocationDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EAN = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Allocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Location = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryDocumentLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    DocumentId = table.Column<int>(nullable: true),
                    EAN = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDocumentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryDocumentLines_DeliveryDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DeliveryDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryDocumentLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "RelocationDocumentLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    EAN = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    RelocationDocumentId = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelocationDocumentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelocationDocumentLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelocationDocumentLines_RelocationDocuments_RelocationDocumentId",
                        column: x => x.RelocationDocumentId,
                        principalTable: "RelocationDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StocksCache",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Location = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocksCache", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocksCache_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_ProductId",
                table: "Allocations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentLines_DocumentId",
                table: "DeliveryDocumentLines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentLines_ProductId",
                table: "DeliveryDocumentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDocumentLines_ProductId",
                table: "ReleaseDocumentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDocumentLines_ReleaseDocumentId",
                table: "ReleaseDocumentLines",
                column: "ReleaseDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RelocationDocumentLines_ProductId",
                table: "RelocationDocumentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelocationDocumentLines_RelocationDocumentId",
                table: "RelocationDocumentLines",
                column: "RelocationDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_StocksCache_ProductId",
                table: "StocksCache",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allocations");

            migrationBuilder.DropTable(
                name: "DeliveryDocumentLines");

            migrationBuilder.DropTable(
                name: "ReleaseDocumentLines");

            migrationBuilder.DropTable(
                name: "RelocationDocumentLines");

            migrationBuilder.DropTable(
                name: "StocksCache");

            migrationBuilder.DropTable(
                name: "DeliveryDocuments");

            migrationBuilder.DropTable(
                name: "ReleaseDocuments");

            migrationBuilder.DropTable(
                name: "RelocationDocuments");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
