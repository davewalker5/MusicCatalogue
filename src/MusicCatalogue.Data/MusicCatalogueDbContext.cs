using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Music;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Data
{
    [ExcludeFromCodeCoverage]
    public class MusicCatalogueDbContext : DbContext
    {
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }

        public MusicCatalogueDbContext(DbContextOptions<MusicCatalogueDbContext> options) : base(options)
        {
        }


        /// <summary>
        /// Initialise the aircraft tracker model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("ARTISTS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");

                entity.HasMany(e => e.Albums)
                    .WithOne(e => e.Artist)
                    .HasForeignKey(e => e.ArtistId);
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("ALBUMS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.ArtistId).IsRequired().HasColumnName("ArtistId");
                entity.Property(e => e.Title).IsRequired().HasColumnName("Title");
                entity.Property(e => e.Released).HasColumnName("Released");
                entity.Property(e => e.Genre).HasColumnName("Genre");
                entity.Property(e => e.CoverUrl).HasColumnName("CoverUrl");

                entity.HasMany(e => e.Tracks)
                    .WithOne(e => e.Album)
                    .HasForeignKey(e => e.AlbumId);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.ToTable("TRACKS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.AlbumId).IsRequired().HasColumnName("AlbumId");
                entity.Property(e => e.Number).HasColumnName("Number");
                entity.Property(e => e.Title).IsRequired().HasColumnName("Title");
                entity.Property(e => e.Duration).HasColumnName("Duration");
            });
        }
    }
}