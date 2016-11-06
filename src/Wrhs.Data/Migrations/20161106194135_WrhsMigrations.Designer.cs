using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Wrhs.Data;

namespace Wrhs.Data.Migrations
{
    [DbContext(typeof(WrhsContext))]
    [Migration("20161106194135_WrhsMigrations")]
    partial class WrhsMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Wrhs.Operations.Allocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location");

                    b.Property<int?>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Allocations");
                });

            modelBuilder.Entity("Wrhs.Operations.Delivery.DeliveryDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullNumber");

                    b.Property<DateTime>("IssueDate");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("DeliveryDocuments");
                });

            modelBuilder.Entity("Wrhs.Operations.Delivery.DeliveryDocumentLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DocumentId");

                    b.Property<string>("EAN");

                    b.Property<int>("Lp");

                    b.Property<int?>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("Remarks");

                    b.Property<string>("SKU");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("ProductId");

                    b.ToTable("DeliveryDocumentLines");
                });

            modelBuilder.Entity("Wrhs.Operations.Release.ReleaseDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullNumber");

                    b.Property<DateTime>("IssueDate");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("ReleaseDocuments");
                });

            modelBuilder.Entity("Wrhs.Operations.Release.ReleaseDocumentLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EAN");

                    b.Property<string>("Location");

                    b.Property<int>("Lp");

                    b.Property<int?>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<int?>("ReleaseDocumentId");

                    b.Property<string>("Remarks");

                    b.Property<string>("SKU");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ReleaseDocumentId");

                    b.ToTable("ReleaseDocumentLines");
                });

            modelBuilder.Entity("Wrhs.Operations.Relocation.RelocationDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullNumber");

                    b.Property<DateTime>("IssueDate");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("RelocationDocuments");
                });

            modelBuilder.Entity("Wrhs.Operations.Relocation.RelocationDocumentLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EAN");

                    b.Property<string>("From");

                    b.Property<int>("Lp");

                    b.Property<int?>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<int?>("RelocationDocumentId");

                    b.Property<string>("Remarks");

                    b.Property<string>("SKU");

                    b.Property<string>("To");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("RelocationDocumentId");

                    b.ToTable("RelocationDocumentLines");
                });

            modelBuilder.Entity("Wrhs.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("EAN");

                    b.Property<string>("Name");

                    b.Property<string>("SKU");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Wrhs.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location");

                    b.Property<int?>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("StocksCache");
                });

            modelBuilder.Entity("Wrhs.Operations.Allocation", b =>
                {
                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Wrhs.Operations.Delivery.DeliveryDocumentLine", b =>
                {
                    b.HasOne("Wrhs.Operations.Delivery.DeliveryDocument", "Document")
                        .WithMany("Lines")
                        .HasForeignKey("DocumentId");

                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Wrhs.Operations.Release.ReleaseDocumentLine", b =>
                {
                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("Wrhs.Operations.Release.ReleaseDocument")
                        .WithMany("Lines")
                        .HasForeignKey("ReleaseDocumentId");
                });

            modelBuilder.Entity("Wrhs.Operations.Relocation.RelocationDocumentLine", b =>
                {
                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("Wrhs.Operations.Relocation.RelocationDocument")
                        .WithMany("Lines")
                        .HasForeignKey("RelocationDocumentId");
                });

            modelBuilder.Entity("Wrhs.Stock", b =>
                {
                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });
        }
    }
}
