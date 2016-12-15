using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wrhs.Data.Migrations
{
    [DbContext(typeof(WrhsContext))]
    [Migration("20161203214500_initial2")]
    partial class initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Wrhs.Delivery.DeliveryDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("DeliveryDocuments");
                });

            modelBuilder.Entity("Wrhs.Delivery.DeliveryDocument+DocumentLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DeliveryDocumentId");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryDocumentId");

                    b.HasIndex("ProductId");

                    b.ToTable("DeliveryDocumentLines");
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

            modelBuilder.Entity("Wrhs.Delivery.DeliveryDocument+DocumentLine", b =>
                {
                    b.HasOne("Wrhs.Delivery.DeliveryDocument")
                        .WithMany("Lines")
                        .HasForeignKey("DeliveryDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wrhs.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
