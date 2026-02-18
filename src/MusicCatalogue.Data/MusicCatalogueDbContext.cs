using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Reporting;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Data
{
    [ExcludeFromCodeCoverage]
    public class MusicCatalogueDbContext : DbContext
    {
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Mood> Moods { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<ArtistMood> ArtistMoods { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<Retailer> Retailers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<JobStatus> JobStatuses { get; set; }
        public virtual DbSet<GenreStatistics> GenreStatistics { get; set; }
        public virtual DbSet<ArtistStatistics> ArtistStatistics { get; set; }
        public virtual DbSet<MonthlySpend> MonthlySpend { get; set; }
        public virtual DbSet<RetailerStatistics> RetailerStatistics { get; set; }
        public virtual DbSet<GenreAlbum> GenreAlbums { get; set; }
        public virtual DbSet<AlbumByPurchaseDate> AlbumsByPurchaseDate { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionAlbum> SessionAlbums { get; set; }

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

            modelBuilder.Entity<Retailer>(entity =>
            {
                entity.ToTable("RETAILERS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
            });

            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.ToTable("EQUIPMENT_TYPES");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("MANUFACTURERS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("GENRES");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("ARTISTS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
                entity.Property(e => e.SearchableName).HasColumnName("SearchableName");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("ALBUMS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.ArtistId).IsRequired().HasColumnName("ArtistId");
                entity.Property(e => e.GenreId).HasColumnName("GenreId");
                entity.Property(e => e.Title).IsRequired().HasColumnName("Title");
                entity.Property(e => e.Released).HasColumnName("Released");
                entity.Property(e => e.CoverUrl).HasColumnName("CoverUrl");
                entity.Property(e => e.Purchased).HasColumnName("Purchased");
                entity.Property(e => e.Price).HasColumnName("Price");
                entity.Property(e => e.RetailerId).HasColumnName("RetailerId");
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

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("EQUIPMENT");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.EquipmentTypeId).IsRequired().HasColumnName("EquipmentTypeId");
                entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerId");
                entity.Property(e => e.Description).IsRequired().HasColumnName("Description");
                entity.Property(e => e.Model).HasColumnName("Model");
                entity.Property(e => e.SerialNumber).HasColumnName("SerialNumber");
                entity.Property(e => e.Purchased).HasColumnName("Purchased");
                entity.Property(e => e.Price).HasColumnName("Price");
                entity.Property(e => e.RetailerId).HasColumnName("RetailerId");
            });

            modelBuilder.Entity<JobStatus>(entity =>
            {
                entity.ToTable("JOB_STATUS");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("name");
                entity.Property(e => e.Parameters).HasColumnName("parameters");
                entity.Property(e => e.Start).IsRequired().HasColumnName("start").HasColumnType("DATETIME");
                entity.Property(e => e.End).HasColumnName("end").HasColumnType("DATETIME");
                entity.Property(e => e.Error).HasColumnName("error");
            });

            modelBuilder.Entity<Mood>(entity =>
            {
                entity.ToTable("MOODS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name");
            });

            modelBuilder.Entity<ArtistMood>(entity =>
            {
                entity.ToTable("ARTIST_MOODS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.HasOne(am => am.Mood).WithMany().HasForeignKey(am => am.MoodId);
                entity.HasOne<Artist>().WithMany(a => a.Moods).HasForeignKey(am => am.ArtistId);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("SESSIONS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("CreatedAt").HasColumnType("DATETIME");
                entity.Property(e => e.Type).IsRequired().HasColumnName("Type");
                entity.Property(e => e.TimeOfDay).IsRequired().HasColumnName("TimeOfDay");
            });

            modelBuilder.Entity<SessionAlbum>(entity =>
            {
                entity.ToTable("SESSION_ALBUMS");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.Position).IsRequired().HasColumnName("Position");
                entity.Property(e => e.SessionId).IsRequired().HasColumnName("SessionId");
                entity.Property(e => e.AlbumId).IsRequired().HasColumnName("AlbumId");
    
                entity.HasOne<Session>().WithMany(s => s.SessionAlbums).HasForeignKey(sa => sa.SessionId);
            });
        }
    }
}