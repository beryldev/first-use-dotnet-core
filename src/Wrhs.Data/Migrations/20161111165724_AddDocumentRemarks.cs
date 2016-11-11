using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    public partial class AddDocumentRemarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "RelocationDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ReleaseDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "DeliveryDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "RelocationDocuments");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ReleaseDocuments");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "DeliveryDocuments");
        }
    }
}
