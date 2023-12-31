﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReceivableApi.Data;

#nullable disable

namespace ReceivableApi.Migrations
{
    [DbContext(typeof(ReceivableApiContext))]
    partial class ReceivableApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.23");

            modelBuilder.Entity("ReceivableApi.Models.Debtor", b =>
                {
                    b.Property<string>("Reference")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address1")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address2")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .HasColumnType("TEXT");

                    b.Property<string>("Town")
                        .HasColumnType("TEXT");

                    b.Property<string>("Zip")
                        .HasColumnType("TEXT");

                    b.HasKey("Reference");

                    b.ToTable("Debtors");
                });

            modelBuilder.Entity("ReceivableApi.Models.Receivable", b =>
                {
                    b.Property<string>("Reference")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Cancelled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ClosedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Due")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Issued")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PaidValue")
                        .HasColumnType("TEXT");

                    b.HasKey("Reference");

                    b.HasIndex("DebtorId");

                    b.ToTable("Receivables");
                });

            modelBuilder.Entity("ReceivableApi.Models.Receivable", b =>
                {
                    b.HasOne("ReceivableApi.Models.Debtor", "Debtor")
                        .WithMany()
                        .HasForeignKey("DebtorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Debtor");
                });
#pragma warning restore 612, 618
        }
    }
}
