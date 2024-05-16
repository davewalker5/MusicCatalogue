﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicCatalogue.Data;

#nullable disable

namespace MusicCatalogue.Data.Migrations
{
    [DbContext(typeof(MusicCatalogueDbContext))]
    [Migration("20240516104534_GenreAlbums")]
    partial class GenreAlbums
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<int>("ArtistId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ArtistId");

                    b.Property<string>("CoverUrl")
                        .HasColumnType("TEXT")
                        .HasColumnName("CoverUrl");

                    b.Property<int?>("GenreId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GenreId");

                    b.Property<bool?>("IsWishListItem")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Price")
                        .HasColumnType("TEXT")
                        .HasColumnName("Price");

                    b.Property<DateTime?>("Purchased")
                        .HasColumnType("TEXT")
                        .HasColumnName("Purchased");

                    b.Property<int?>("Released")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Released");

                    b.Property<int?>("RetailerId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("RetailerId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("GenreId");

                    b.HasIndex("RetailerId");

                    b.ToTable("ALBUMS", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<string>("SearchableName")
                        .HasColumnType("TEXT")
                        .HasColumnName("SearchableName");

                    b.HasKey("Id");

                    b.ToTable("ARTISTS", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Description");

                    b.Property<int>("EquipmentTypeId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("EquipmentTypeId");

                    b.Property<bool?>("IsWishListItem")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ManufacturerId");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT")
                        .HasColumnName("Model");

                    b.Property<decimal?>("Price")
                        .HasColumnType("TEXT")
                        .HasColumnName("Price");

                    b.Property<DateTime?>("Purchased")
                        .HasColumnType("TEXT")
                        .HasColumnName("Purchased");

                    b.Property<int?>("RetailerId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("RetailerId");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("TEXT")
                        .HasColumnName("SerialNumber");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentTypeId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("RetailerId");

                    b.ToTable("EQUIPMENT", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.EquipmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("EQUIPMENT_TYPES", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("GENRES", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.JobStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime?>("End")
                        .HasColumnType("DATETIME")
                        .HasColumnName("end");

                    b.Property<string>("Error")
                        .HasColumnType("TEXT")
                        .HasColumnName("error");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<string>("Parameters")
                        .HasColumnType("TEXT")
                        .HasColumnName("parameters");

                    b.Property<DateTime>("Start")
                        .HasColumnType("DATETIME")
                        .HasColumnName("start");

                    b.HasKey("Id");

                    b.ToTable("JOB_STATUS", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("MANUFACTURERS", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Retailer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Address1")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address2")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("County")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Name");

                    b.Property<string>("PostCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Town")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebSite")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RETAILERS", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<int>("AlbumId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("AlbumId");

                    b.Property<int?>("Duration")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Duration");

                    b.Property<int?>("Number")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Number");

                    b.Property<DateTime?>("Purchased")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.ToTable("TRACKS", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Password");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.ToTable("USER", (string)null);
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Reporting.ArtistStatistics", b =>
                {
                    b.Property<int?>("Albums")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Spend")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Tracks")
                        .HasColumnType("INTEGER");

                    b.ToTable("ArtistStatistics");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Reporting.GenreAlbum", b =>
                {
                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Purchased")
                        .HasColumnType("TEXT");

                    b.Property<string>("Retailer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("GenreAlbums");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Reporting.GenreStatistics", b =>
                {
                    b.Property<int?>("Albums")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Artists")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Spend")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Tracks")
                        .HasColumnType("INTEGER");

                    b.ToTable("GenreStatistics");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Reporting.MonthlySpend", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Spend")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.ToTable("MonthlySpend");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Reporting.RetailerStatistics", b =>
                {
                    b.Property<int?>("Albums")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Artists")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Spend")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Tracks")
                        .HasColumnType("INTEGER");

                    b.ToTable("RetailerStatistics");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Album", b =>
                {
                    b.HasOne("MusicCatalogue.Entities.Database.Artist", null)
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicCatalogue.Entities.Database.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");

                    b.HasOne("MusicCatalogue.Entities.Database.Retailer", "Retailer")
                        .WithMany()
                        .HasForeignKey("RetailerId");

                    b.Navigation("Genre");

                    b.Navigation("Retailer");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Equipment", b =>
                {
                    b.HasOne("MusicCatalogue.Entities.Database.EquipmentType", "EquipmentType")
                        .WithMany()
                        .HasForeignKey("EquipmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicCatalogue.Entities.Database.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicCatalogue.Entities.Database.Retailer", "Retailer")
                        .WithMany()
                        .HasForeignKey("RetailerId");

                    b.Navigation("EquipmentType");

                    b.Navigation("Manufacturer");

                    b.Navigation("Retailer");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Track", b =>
                {
                    b.HasOne("MusicCatalogue.Entities.Database.Album", null)
                        .WithMany("Tracks")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Album", b =>
                {
                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Artist", b =>
                {
                    b.Navigation("Albums");
                });
#pragma warning restore 612, 618
        }
    }
}
