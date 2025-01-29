﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebStokYApp.Models;

#nullable disable

namespace WebStokYApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241230125051_AddProductIDToLogs")]
    partial class AddProductIDToLogs
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebStokYApp.Models.Entities.Admin", b =>
                {
                    b.Property<int>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AdminID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Carts", (string)null);
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<decimal?>("Budget")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CustomerType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("123");

                    b.Property<decimal?>("TotalSpent")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers", (string)null);

                    b.HasData(
                        new
                        {
                            CustomerID = 1,
                            Budget = 1445m,
                            CustomerName = "Customer1",
                            CustomerType = "Premium",
                            Password = "1231",
                            TotalSpent = 0m
                        },
                        new
                        {
                            CustomerID = 2,
                            Budget = 2954m,
                            CustomerName = "Customer2",
                            CustomerType = "Premium",
                            Password = "1232",
                            TotalSpent = 0m
                        },
                        new
                        {
                            CustomerID = 3,
                            Budget = 1825m,
                            CustomerName = "Customer3",
                            CustomerType = "Normal",
                            Password = "1233",
                            TotalSpent = 0m
                        },
                        new
                        {
                            CustomerID = 4,
                            Budget = 906m,
                            CustomerName = "Customer4",
                            CustomerType = "Premium",
                            Password = "1234",
                            TotalSpent = 0m
                        },
                        new
                        {
                            CustomerID = 5,
                            Budget = 849m,
                            CustomerName = "Customer5",
                            CustomerType = "Premium",
                            Password = "1235",
                            TotalSpent = 0m
                        },
                        new
                        {
                            CustomerID = 6,
                            Budget = 641m,
                            CustomerName = "Customer6",
                            CustomerType = "Normal",
                            Password = "1236",
                            TotalSpent = 0m
                        });
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Log", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogID"));

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogDetails")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("LogType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("OrderID")
                        .HasColumnType("int");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("LogID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("Logs", (string)null);
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OrderDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalPrice")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<decimal?>("Price")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            ProductID = 1,
                            Price = 100m,
                            ProductName = "Product1",
                            Stock = 500
                        },
                        new
                        {
                            ProductID = 2,
                            Price = 50m,
                            ProductName = "Product2",
                            Stock = 10
                        },
                        new
                        {
                            ProductID = 3,
                            Price = 45m,
                            ProductName = "Product3",
                            Stock = 200
                        },
                        new
                        {
                            ProductID = 4,
                            Price = 75m,
                            ProductName = "Product4",
                            Stock = 75
                        },
                        new
                        {
                            ProductID = 5,
                            Price = 500m,
                            ProductName = "Product5",
                            Stock = 0
                        });
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Cart", b =>
                {
                    b.HasOne("WebStokYApp.Models.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebStokYApp.Models.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Log", b =>
                {
                    b.HasOne("WebStokYApp.Models.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WebStokYApp.Models.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WebStokYApp.Models.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Customer");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebStokYApp.Models.Entities.Order", b =>
                {
                    b.HasOne("WebStokYApp.Models.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebStokYApp.Models.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
