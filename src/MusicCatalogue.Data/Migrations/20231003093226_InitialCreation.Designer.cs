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
    [Migration("20231003093226_InitialCreation")]
    partial class InitialCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Album", b =>
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

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Artist", b =>
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

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Track", b =>
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

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Album", b =>
                {
                    b.HasOne("MusicCatalogue.Entities.Music.Artist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Track", b =>
                {
                    b.HasOne("MusicCatalogue.Entities.Music.Album", "Album")
                        .WithMany("Tracks")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Album", b =>
                {
                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("MusicCatalogue.Entities.Music.Artist", b =>
                {
                    b.Navigation("Albums");
                });
#pragma warning restore 612, 618
        }
    }
}
