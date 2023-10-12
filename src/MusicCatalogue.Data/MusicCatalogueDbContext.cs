using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Data
{
    [ExcludeFromCodeCoverage]
    public class MusicCatalogueDbContext : DbContext
    {
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public MusicCatalogueDbContext(DbContextOptions<MusicCatalogueDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }


        /// <summary>
        /// Initialise the aircraft tracker model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.UserName).IsRequired().HasColumnName("UserName");
                entity.Property(e => e.Password).IsRequired().HasColumnName("Password");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("ARTISTS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
                entity.Ignore(e => e.AlbumCount);
                entity.Ignore(e => e.TrackCount);
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
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.ToTable("TRACKS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.AlbumId).IsRequired().HasColumnName("AlbumId");
                entity.Property(e => e.Number).HasColumnName("Number");
                entity.Property(e => e.Title).IsRequired().HasColumnName("Title");
                entity.Property(e => e.Duration).HasColumnName("Duration");
                entity.Ignore(e => e.FormattedDuration);
            });
        }
    }
}