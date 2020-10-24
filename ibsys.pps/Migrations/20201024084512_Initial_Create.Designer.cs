﻿// <auto-generated />
using System;
using IBSYS.PPS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IBSYS.PPS.Migrations
{
    [DbContext(typeof(IbsysDatabaseContext))]
    [Migration("20201024084512_Initial_Create")]
    partial class Initial_Create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IBSYS.PPS.Models.BillOfMaterial", b =>
                {
                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProductName");

                    b.ToTable("BillOfMaterials");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Generated.Article", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pct")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Startamount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stockvalue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StockValuesFromLastPeriod");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Generated.Batch", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cycletime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderID");

                    b.ToTable("Batch");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Generated.Order", b =>
                {
                    b.Property<int?>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Article")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Averageunitcosts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Entirecosts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Materialcosts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ordercosts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Orderperiod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Period")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Piececosts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quantity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderID");

                    b.ToTable("FutureInwardStockMovement");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.LaborAndMachineCosts", b =>
                {
                    b.Property<int>("Workplace")
                        .HasColumnType("int");

                    b.Property<double>("IdleTimeMachineCosts")
                        .HasColumnType("float");

                    b.Property<double>("LaborCostsFirstShift")
                        .HasColumnType("float");

                    b.Property<double>("LaborCostsOvertime")
                        .HasColumnType("float");

                    b.Property<double>("LaborCostsSecondShift")
                        .HasColumnType("float");

                    b.Property<double>("LaborCostsThirdShift")
                        .HasColumnType("float");

                    b.Property<double>("ProductiveMachineCosts")
                        .HasColumnType("float");

                    b.HasKey("Workplace");

                    b.ToTable("LaborAndMachineCosts");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Material", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BillOfMaterialProductName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaterialName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentMaterialId")
                        .HasColumnType("int");

                    b.Property<int>("QuantityNeeded")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BillOfMaterialProductName");

                    b.HasIndex("ParentMaterialId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Stock", b =>
                {
                    b.Property<string>("ItemNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ItemValue")
                        .HasColumnType("float");

                    b.Property<int>("QuantityInStock")
                        .HasColumnType("int");

                    b.Property<string>("Usage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ItemNumber");

                    b.ToTable("InitialStock");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Stock");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.PurchasedItems", b =>
                {
                    b.HasBaseType("IBSYS.PPS.Models.Stock");

                    b.Property<double>("Deviation")
                        .HasColumnType("float");

                    b.Property<int>("DiscountQuantity")
                        .HasColumnType("int");

                    b.Property<double>("OrderCosts")
                        .HasColumnType("float");

                    b.Property<double>("ProcureLeadTime")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("PurchasedItems");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.SelfProductionItems", b =>
                {
                    b.HasBaseType("IBSYS.PPS.Models.Stock");

                    b.HasDiscriminator().HasValue("SelfProductionItems");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Generated.Batch", b =>
                {
                    b.HasOne("IBSYS.PPS.Models.Generated.Order", null)
                        .WithMany("Batch")
                        .HasForeignKey("OrderID");
                });

            modelBuilder.Entity("IBSYS.PPS.Models.Material", b =>
                {
                    b.HasOne("IBSYS.PPS.Models.BillOfMaterial", null)
                        .WithMany("RequiredMaterials")
                        .HasForeignKey("BillOfMaterialProductName");

                    b.HasOne("IBSYS.PPS.Models.Material", "ParentMaterial")
                        .WithMany("MaterialNeeded")
                        .HasForeignKey("ParentMaterialId");
                });
#pragma warning restore 612, 618
        }
    }
}