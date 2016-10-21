using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Wrhs.Data;

namespace Wrhs.Data.Migrations
{
    [DbContext(typeof(WrhsContext))]
    [Migration("20161021201832_AllocationsMigration")]
    partial class AllocationsMigration
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

            modelBuilder.Entity("Wrhs.Operations.Allocation", b =>
                {
                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });
        }
    }
}
