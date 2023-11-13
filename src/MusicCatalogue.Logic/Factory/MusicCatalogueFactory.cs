using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.Logic.Database;
using MusicCatalogue.Logic.DataExchange;
using MusicCatalogue.Logic.Reporting;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.Factory
{
    public class MusicCatalogueFactory : IMusicCatalogueFactory
    {
        private readonly Lazy<IArtistManager> _artists;
        private readonly Lazy<IAlbumManager> _albums;
        private readonly Lazy<ITrackManager> _tracks;
        private readonly Lazy<IRetailerManager> _retailers;
        private readonly Lazy<IUserManager> _users;
        private readonly Lazy<IImporter> _importer;
        private readonly Lazy<IExporter> _csvExporter;
        private readonly Lazy<IExporter> _xlsxExporter;
        private readonly Lazy<IStatisticsManager> _statistics;
        private readonly Lazy<IJobStatusManager> _jobStatuses;
        private readonly Lazy<IWishListBasedReport<GenreStatistics>> _genreStatistics;

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<GenreStatistics> GenreStatistics { get { return _genreStatistics.Value; } }

        public DbContext Context { get; private set; }
        public IArtistManager Artists { get { return _artists.Value; } }
        public IAlbumManager Albums { get { return _albums.Value; } }
        public ITrackManager Tracks { get { return _tracks.Value; } }
        public IRetailerManager Retailers { get { return _retailers.Value; } }
        public IJobStatusManager JobStatuses { get { return _jobStatuses.Value; } }
        public IUserManager Users { get { return _users.Value; } }

        [ExcludeFromCodeCoverage]
        public IImporter Importer { get {  return _importer.Value; } }

        [ExcludeFromCodeCoverage]
        public IExporter CsvExporter { get { return _csvExporter.Value; } }

        [ExcludeFromCodeCoverage]
        public IExporter XlsxExporter { get { return _xlsxExporter.Value; } }
        public IStatisticsManager Statistics { get { return _statistics.Value; } }

        public MusicCatalogueFactory(MusicCatalogueDbContext context)
        {
            Context = context;
            _artists = new Lazy<IArtistManager>(() => new ArtistManager(context));
            _albums = new Lazy<IAlbumManager>(() => new AlbumManager(this));
            _tracks = new Lazy<ITrackManager>(() => new TrackManager(context));
            _retailers = new Lazy<IRetailerManager>(() => new RetailerManager(this));
            _jobStatuses = new Lazy<IJobStatusManager>(() => new JobStatusManager(context));
            _users = new Lazy<IUserManager>(() => new UserManager(context));
            _importer = new Lazy<IImporter>(() => new CsvImporter(this));
            _csvExporter = new Lazy<IExporter>(() => new CsvExporter(this));
            _xlsxExporter = new Lazy<IExporter>(() => new XlsxExporter(this));
            _statistics = new Lazy<IStatisticsManager>(() => new StatisticsManager(this));
            _genreStatistics = new Lazy<IWishListBasedReport<GenreStatistics>>(() => new WishListBasedReport<GenreStatistics>(context));
        }
    }
}