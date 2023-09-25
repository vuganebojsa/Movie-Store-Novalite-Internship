﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieStore.Infrastructure;

#nullable disable

namespace MovieStore.Infrastructure.Migrations
{
    [DbContext(typeof(MovieStoreContext))]
    [Migration("20230908073035_Initial2")]
    partial class Initial2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieStore.Core.Model.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MovieStore.Core.Model.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LicensingType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieStore.Core.Model.PurchasedMovie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfPurchase")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("MovieId");

                    b.ToTable("PurchasedMovies");
                });

            modelBuilder.Entity("MovieStore.Core.Model.Customer", b =>
                {
                    b.OwnsOne("MovieStore.Core.Model.ExpirationDate", "Expiration", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime?>("Date")
                                .HasColumnType("datetime2")
                                .HasColumnName("Expiration");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("MovieStore.Core.Model.MoneySpent", "Spent", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Spent");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("MovieStore.Core.Model.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("EmailAddress")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Email");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Expiration")
                        .IsRequired();

                    b.Navigation("Spent")
                        .IsRequired();
                });

            modelBuilder.Entity("MovieStore.Core.Model.PurchasedMovie", b =>
                {
                    b.HasOne("MovieStore.Core.Model.Customer", "Customer")
                        .WithMany("PurchasedMovies")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieStore.Core.Model.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("MovieStore.Core.Model.ExpirationDate", "ExpirationDate", b1 =>
                        {
                            b1.Property<Guid>("PurchasedMovieId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime?>("Date")
                                .HasColumnType("datetime2")
                                .HasColumnName("Expiration");

                            b1.HasKey("PurchasedMovieId");

                            b1.ToTable("PurchasedMovies");

                            b1.WithOwner()
                                .HasForeignKey("PurchasedMovieId");
                        });

                    b.OwnsOne("MovieStore.Core.Model.MoneySpent", "Price", b1 =>
                        {
                            b1.Property<Guid>("PurchasedMovieId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Amount");

                            b1.HasKey("PurchasedMovieId");

                            b1.ToTable("PurchasedMovies");

                            b1.WithOwner()
                                .HasForeignKey("PurchasedMovieId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("ExpirationDate")
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Price")
                        .IsRequired();
                });

            modelBuilder.Entity("MovieStore.Core.Model.Customer", b =>
                {
                    b.Navigation("PurchasedMovies");
                });
#pragma warning restore 612, 618
        }
    }
}