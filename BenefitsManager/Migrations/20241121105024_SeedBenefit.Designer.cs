﻿// <auto-generated />
using System;
using BenefitsManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BenefitsManager.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20241121105024_SeedBenefit")]
    partial class SeedBenefit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BenefitsManager.Models.Benefit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("DiscountPercentage")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Benefits");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DiscountPercentage = 0f,
                            Name = "Sem Benefício"
                        });
                });

            modelBuilder.Entity("BenefitsManager.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BenefitId")
                        .HasColumnType("integer");

                    b.Property<float>("FinalValue")
                        .HasColumnType("real");

                    b.Property<float>("InitialValue")
                        .HasColumnType("real");

                    b.Property<int>("TaxpayerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BenefitId");

                    b.HasIndex("TaxpayerId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BenefitsManager.Models.Taxpayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("OpeningDate")
                        .HasColumnType("date");

                    b.Property<string>("TaxationRegime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Taxpayers");
                });

            modelBuilder.Entity("BenefitsManager.Models.TaxpayerBenefit", b =>
                {
                    b.Property<int>("TaxpayerId")
                        .HasColumnType("integer");

                    b.Property<int>("BenefitId")
                        .HasColumnType("integer");

                    b.HasKey("TaxpayerId", "BenefitId");

                    b.HasIndex("BenefitId");

                    b.ToTable("TaxpayerBenefits");
                });

            modelBuilder.Entity("BenefitsManager.Models.Payment", b =>
                {
                    b.HasOne("BenefitsManager.Models.Benefit", "Benefit")
                        .WithMany()
                        .HasForeignKey("BenefitId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BenefitsManager.Models.Taxpayer", "Taxpayer")
                        .WithMany()
                        .HasForeignKey("TaxpayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Benefit");

                    b.Navigation("Taxpayer");
                });

            modelBuilder.Entity("BenefitsManager.Models.TaxpayerBenefit", b =>
                {
                    b.HasOne("BenefitsManager.Models.Benefit", "Benefit")
                        .WithMany("TaxpayerBenefits")
                        .HasForeignKey("BenefitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BenefitsManager.Models.Taxpayer", "Taxpayer")
                        .WithMany("TaxpayerBenefits")
                        .HasForeignKey("TaxpayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benefit");

                    b.Navigation("Taxpayer");
                });

            modelBuilder.Entity("BenefitsManager.Models.Benefit", b =>
                {
                    b.Navigation("TaxpayerBenefits");
                });

            modelBuilder.Entity("BenefitsManager.Models.Taxpayer", b =>
                {
                    b.Navigation("TaxpayerBenefits");
                });
#pragma warning restore 612, 618
        }
    }
}
