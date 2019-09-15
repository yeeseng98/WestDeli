﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WestDeli.Models;

namespace WestDeli.Migrations
{
    [DbContext(typeof(WestDeliContext))]
    [Migration("20190915095607_asd")]
    partial class asd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WestDeli.Models.Dish", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("DishName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ImgLink");

                    b.Property<int>("PrepTime");

                    b.Property<int>("Price");

                    b.HasKey("ID");

                    b.ToTable("Dish");
                });

            modelBuilder.Entity("WestDeli.Models.OrderObject", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("DishName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Identifier")
                        .IsRequired();

                    b.Property<int>("Portion");

                    b.Property<int>("PrepTime");

                    b.Property<int>("Price");

                    b.Property<int?>("TransactionID");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("TransactionID");

                    b.ToTable("OrderObject");
                });

            modelBuilder.Entity("WestDeli.Models.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Identifier");

                    b.Property<string>("Status");

                    b.Property<string>("TotalPrice");

                    b.Property<string>("TransactDate");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("WestDeli.Models.OrderObject", b =>
                {
                    b.HasOne("WestDeli.Models.Transaction")
                        .WithMany("items")
                        .HasForeignKey("TransactionID");
                });
#pragma warning restore 612, 618
        }
    }
}
