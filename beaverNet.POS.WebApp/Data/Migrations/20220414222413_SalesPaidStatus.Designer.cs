﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using beaverNet.POS.WebApp.Data;

namespace beaverNet.POS.WebApp.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220414222413_SalesPaidStatus")]
    partial class SalesPaidStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address2")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateofBirthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.GoodsReceive", b =>
                {
                    b.Property<Guid>("GoodsReceiveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("GoodsReceiveDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PurchaseOrderId")
                        .HasColumnType("TEXT");

                    b.HasKey("GoodsReceiveId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("GoodsReceive");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.GoodsReceiveLine", b =>
                {
                    b.Property<Guid>("GoodsReceiveLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GoodsReceiveId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PurchaseOrderLineId")
                        .HasColumnType("TEXT");

                    b.Property<int>("QtyPurchase")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QtyReceive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QtyReceived")
                        .HasColumnType("INTEGER");

                    b.HasKey("GoodsReceiveLineId");

                    b.HasIndex("GoodsReceiveId");

                    b.HasIndex("ProductId");

                    b.ToTable("GoodsReceiveLine");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.InvenTran", b =>
                {
                    b.Property<Guid>("InvenTranId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("InvenTranDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("TranSourceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TranSourceNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("TranSourceType")
                        .HasColumnType("TEXT");

                    b.HasKey("InvenTranId");

                    b.HasIndex("ProductId");

                    b.ToTable("InvenTran");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.MedicalRecord", b =>
                {
                    b.Property<Guid>("MedicalRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Diagnosis")
                        .HasColumnType("TEXT");

                    b.Property<string>("PyshicalCheck")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("RecordDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SalesOrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Therapy")
                        .HasColumnType("TEXT");

                    b.HasKey("MedicalRecordId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SalesOrderId");

                    b.ToTable("MedicalRecord");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PricePurchase")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PriceSell")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.PurchaseOrder", b =>
                {
                    b.Property<Guid>("PurchaseOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("PurchaseOrderDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VendorId")
                        .HasColumnType("TEXT");

                    b.HasKey("PurchaseOrderId");

                    b.HasIndex("VendorId");

                    b.ToTable("PurchaseOrder");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.PurchaseOrderLine", b =>
                {
                    b.Property<Guid>("PurchaseOrderLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Discount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PurchaseOrderId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Total")
                        .HasColumnType("TEXT");

                    b.HasKey("PurchaseOrderLineId");

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("PurchaseOrderLine");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.SalesOrder", b =>
                {
                    b.Property<Guid>("SalesOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("PaidStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("SalesOrderDate")
                        .HasColumnType("TEXT");

                    b.HasKey("SalesOrderId");

                    b.HasIndex("CustomerId");

                    b.ToTable("SalesOrder");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.SalesOrderLine", b =>
                {
                    b.Property<Guid>("SalesOrderLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Discount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SalesOrderId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Total")
                        .HasColumnType("TEXT");

                    b.HasKey("SalesOrderLineId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesOrderId");

                    b.ToTable("SalesOrderLine");
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.Vendor", b =>
                {
                    b.Property<Guid>("VendorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address2")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.HasKey("VendorId");

                    b.ToTable("Vendor");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.GoodsReceive", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.PurchaseOrder", "PurchaseOrder")
                        .WithMany()
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.GoodsReceiveLine", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.GoodsReceive", "GoodsReceive")
                        .WithMany()
                        .HasForeignKey("GoodsReceiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.InvenTran", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.MedicalRecord", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("beaverNet.POS.WebApp.Models.POS.SalesOrder", "SalesOrder")
                        .WithMany()
                        .HasForeignKey("SalesOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.PurchaseOrder", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.PurchaseOrderLine", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("beaverNet.POS.WebApp.Models.POS.PurchaseOrder", "PurchaseOrder")
                        .WithMany("PurchaseOrderLine")
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.SalesOrder", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("beaverNet.POS.WebApp.Models.POS.SalesOrderLine", b =>
                {
                    b.HasOne("beaverNet.POS.WebApp.Models.POS.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("beaverNet.POS.WebApp.Models.POS.SalesOrder", "SalesOrder")
                        .WithMany("SalesOrderLine")
                        .HasForeignKey("SalesOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}