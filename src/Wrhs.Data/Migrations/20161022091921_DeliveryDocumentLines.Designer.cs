using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Wrhs.Data;

namespace Wrhs.Data.Migrations
{
    [DbContext(typeof(WrhsContext))]
    [Migration("20161022091921_DeliveryDocumentLines")]
    partial class DeliveryDocumentLines
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Wrhs.Documents.DocumentLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DeliveryDocumentId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("EAN");

                    b.Property<int?>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("Remarks");

                    b.Property<string>("SKU");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryDocumentId");

                    b.HasIndex("ProductId");

                    b.ToTable("DocumentLine");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DocumentLine");
                });

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

                    b.HasKey("Id");

                    b.ToTable("DeliveryDocuments");
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

            modelBuilder.Entity("Wrhs.Operations.Delivery.DeliveryDocumentLine", b =>
                {
                    b.HasBaseType("Wrhs.Documents.DocumentLine");


                    b.ToTable("DeliveryDocumentLine");

                    b.HasDiscriminator().HasValue("DeliveryDocumentLine");
                });

            modelBuilder.Entity("Wrhs.Documents.DocumentLine", b =>
                {
                    b.HasOne("Wrhs.Operations.Delivery.DeliveryDocument")
                        .WithMany("Lines")
                        .HasForeignKey("DeliveryDocumentId");

                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Wrhs.Operations.Allocation", b =>
                {
                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });
        }
    }
}
