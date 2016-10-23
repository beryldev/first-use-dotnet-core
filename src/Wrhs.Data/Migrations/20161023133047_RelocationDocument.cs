using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Wrhs.Data.Migrations
{
    public partial class RelocationDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentLine");

            migrationBuilder.CreateTable(
                name: "DeliveryDocumentLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "RelocationDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullNumber = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelocationDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelocationDocumentLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Products",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DeliveryDocuments",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Allocations",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentLines_DocumentId",
                table: "DeliveryDocumentLines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDocumentLines_ProductId",
                table: "DeliveryDocumentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelocationDocumentLines_ProductId",
                table: "RelocationDocumentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelocationDocumentLines_RelocationDocumentId",
                table: "RelocationDocumentLines",
                column: "RelocationDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryDocumentLines");

            migrationBuilder.DropTable(
                name: "RelocationDocumentLines");

            migrationBuilder.DropTable(
                name: "RelocationDocuments");

            migrationBuilder.CreateTable(
                name: "DocumentLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    DeliveryDocumentId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    EAN = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true),
                    DocumentId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_DocumentLine_DeliveryDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DeliveryDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Products",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DeliveryDocuments",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Allocations",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLine_DeliveryDocumentId",
                table: "DocumentLine",
                column: "DeliveryDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLine_ProductId",
                table: "DocumentLine",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLine_DocumentId",
                table: "DocumentLine",
                column: "DocumentId");
        }
    }
}
