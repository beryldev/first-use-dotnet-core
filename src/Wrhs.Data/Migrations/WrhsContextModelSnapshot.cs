using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Wrhs.Data;
using Wrhs.Common;

namespace Wrhs.Data.Migrations
{
    [DbContext(typeof(WrhsContext))]
    partial class WrhsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Wrhs.Common.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullNumber");

                    b.Property<DateTime>("IssueDate");

                    b.Property<int>("Month");

                    b.Property<int>("Number");

                    b.Property<int>("State");

                    b.Property<int>("Type");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Wrhs.Common.DocumentLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DocumentId");

                    b.Property<string>("DstLocation");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("SrcLocation");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("ProductId");

                    b.ToTable("DocumentLines");
                });

            modelBuilder.Entity("Wrhs.Common.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DocumentId");

                    b.Property<string>("OperationGuid");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("Wrhs.Common.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Confirmed");

                    b.Property<string>("Location");

                    b.Property<int>("OperationId");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("OperationId");

                    b.HasIndex("ProductId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("Wrhs.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Wrhs.Common.DocumentLine", b =>
                {
                    b.HasOne("Wrhs.Common.Document", "Document")
                        .WithMany("Lines")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wrhs.Common.Operation", b =>
                {
                    b.HasOne("Wrhs.Common.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wrhs.Common.Shift", b =>
                {
                    b.HasOne("Wrhs.Common.Operation", "Operation")
                        .WithMany("Shifts")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
