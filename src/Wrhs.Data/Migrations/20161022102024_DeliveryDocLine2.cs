using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    public partial class DeliveryDocLine2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "DocumentLine",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLine_DocumentId",
                table: "DocumentLine",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentLine_DeliveryDocuments_DocumentId",
                table: "DocumentLine",
                column: "DocumentId",
                principalTable: "DeliveryDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentLine_DeliveryDocuments_DocumentId",
                table: "DocumentLine");

            migrationBuilder.DropIndex(
                name: "IX_DocumentLine_DocumentId",
                table: "DocumentLine");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "DocumentLine");
        }
    }
}
