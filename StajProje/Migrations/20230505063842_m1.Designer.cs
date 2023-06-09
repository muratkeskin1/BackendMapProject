﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StajProje.Helper;

namespace StajProje.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230505063842_m1")]
    partial class m1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            modelBuilder.Entity("StajProje.Model.ATM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ATMId")
                        .HasColumnType("integer");

                    b.Property<bool>("Euro")
                        .HasColumnType("boolean");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<bool>("ParaYatırma")
                        .HasColumnType("boolean");

                    b.Property<bool>("ParaÇekme")
                        .HasColumnType("boolean");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<bool>("TL")
                        .HasColumnType("boolean");

                    b.Property<bool>("USD")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("ATMs");
                });

            modelBuilder.Entity("StajProje.Model.AtmDeliveryHistory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("AtmId")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot10")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot100")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot20")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot200")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot50")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("AtmDeliveryHistories");
                });

            modelBuilder.Entity("StajProje.Model.AtmHistory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ATMId")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot10")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot100")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot20")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot200")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot50")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ProcessType")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("AtmHistories");
                });

            modelBuilder.Entity("StajProje.Model.Capacity", b =>
                {
                    b.Property<int>("CapacityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ATMId")
                        .HasColumnType("integer");

                    b.Property<int>("MevcutBanknot10")
                        .HasColumnType("integer");

                    b.Property<int>("MevcutBanknot100")
                        .HasColumnType("integer");

                    b.Property<int>("MevcutBanknot20")
                        .HasColumnType("integer");

                    b.Property<int>("MevcutBanknot200")
                        .HasColumnType("integer");

                    b.Property<int>("MevcutBanknot50")
                        .HasColumnType("integer");

                    b.HasKey("CapacityId");

                    b.HasIndex("ATMId")
                        .IsUnique();

                    b.ToTable("Capacities");
                });

            modelBuilder.Entity("StajProje.Model.DeliveryHistory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<double>("TotalDistance")
                        .HasColumnType("double precision");

                    b.Property<int>("TotalRoute")
                        .HasColumnType("integer");

                    b.Property<double>("TotalTimeMinute")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("DeliveryHistories");
                });

            modelBuilder.Entity("StajProje.Model.StdCapacity", b =>
                {
                    b.Property<int>("StdCapacityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ATMId")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot10")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot100")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot20")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot200")
                        .HasColumnType("integer");

                    b.Property<int>("Banknot50")
                        .HasColumnType("integer");

                    b.HasKey("StdCapacityId");

                    b.HasIndex("ATMId")
                        .IsUnique();

                    b.ToTable("StdCapacities");
                });

            modelBuilder.Entity("StajProje.Model.UserLogin", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("StajProje.Model.Capacity", b =>
                {
                    b.HasOne("StajProje.Model.ATM", "ATM")
                        .WithOne("Capacity")
                        .HasForeignKey("StajProje.Model.Capacity", "ATMId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ATM");
                });

            modelBuilder.Entity("StajProje.Model.StdCapacity", b =>
                {
                    b.HasOne("StajProje.Model.ATM", "ATM")
                        .WithOne("StdCapacity")
                        .HasForeignKey("StajProje.Model.StdCapacity", "ATMId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ATM");
                });

            modelBuilder.Entity("StajProje.Model.ATM", b =>
                {
                    b.Navigation("Capacity");

                    b.Navigation("StdCapacity");
                });
#pragma warning restore 612, 618
        }
    }
}
