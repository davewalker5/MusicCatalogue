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
    [Migration("20231110032647_WishList")]
    partial class WishList
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

                    b.Property<string>("Genre")
                        .HasColumnType("TEXT")
                        .HasColumnName("Genre");

                    b.Property<bool?>("IsWishListItem")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Released")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Released");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

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

                    b.HasKey("Id");

                    b.ToTable("ARTISTS", (string)null);
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

            modelBuilder.Entity("MusicCatalogue.Entities.Database.Album", b =>
                {
                    b.HasOne("MusicCatalogue.Entities.Database.Artist", null)
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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