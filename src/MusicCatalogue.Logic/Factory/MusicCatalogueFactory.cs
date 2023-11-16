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
        private readonly Lazy<IGenreManager> _genres;
        private readonly Lazy<IArtistManager> _artists;
        private readonly Lazy<IAlbumManager> _albums;
        private readonly Lazy<ITrackManager> _tracks;
        private readonly Lazy<IRetailerManager> _retailers;
        private readonly Lazy<IUserManager> _users;
        private readonly Lazy<IImporter> _importer;
        private readonly Lazy<IExporter> _csvExporter;
        private readonly Lazy<IExporter> _xlsxExporter;
        private readonly Lazy<IJobStatusManager> _jobStatuses;
        private readonly Lazy<ISearchManager> _searchManager;
        private readonly Lazy<IWishListBasedReport<GenreStatistics>> _genreStatistics;
        private readonly Lazy<IWishListBasedReport<ArtistStatistics>> _artistStatistics;
        private readonly Lazy<IWishListBasedReport<MonthlySpend>> _monthlySpend;

        public DbContext Context { get; private set; }
        public IGenreManager Genres { get { return _genres.Value; } }
        public IArtistManager Artists { get { return _artists.Value; } }
        public IAlbumManager Albums { get { return _albums.Value; } }
        public ITrackManager Tracks { get { return _tracks.Value; } }
        public IRetailerManager Retailers { get { return _retailers.Value; } }
        public IJobStatusManager JobStatuses { get { return _jobStatuses.Value; } }
        public ISearchManager Search { get { return _searchManager.Value; } }
        public IUserManager Users { get { return _users.Value; } }
        public IImporter Importer { get {  return _importer.Value; } }
        public IExporter CsvExporter { get { return _csvExporter.Value; } }
        public IExporter XlsxExporter { get { return _xlsxExporter.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<GenreStatistics> GenreStatistics { get { return _genreStatistics.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<ArtistStatistics> ArtistStatistics { get { return _artistStatistics.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<MonthlySpend> MonthlySpend { get { return _monthlySpend.Value; } }

        public MusicCatalogueFactory(MusicCatalogueDbContext context)
        {
            Context = context;
            _genres = new Lazy<IGenreManager>(() => new GenreManager(this));
            _artists = new Lazy<IArtistManager>(() => new ArtistManager(this));
            _albums = new Lazy<IAlbumManager>(() => new AlbumManager(this));
            _tracks = new Lazy<ITrackManager>(() => new TrackManager(this));
            _retailers = new Lazy<IRetailerManager>(() => new RetailerManager(this));
            _jobStatuses = new Lazy<IJobStatusManager>(() => new JobStatusManager(this));
            _searchManager = new Lazy<ISearchManager>(() => new SearchManager(this));
            _users = new Lazy<IUserManager>(() => new UserManager(this));
            _importer = new Lazy<IImporter>(() => new CsvImporter(this));
            _csvExporter = new Lazy<IExporter>(() => new CsvExporter(this));
            _xlsxExporter = new Lazy<IExporter>(() => new XlsxExporter(this));
            _genreStatistics = new Lazy<IWishListBasedReport<GenreStatistics>>(() => new WishListBasedReport<GenreStatistics>(context));
            _artistStatistics = new Lazy<IWishListBasedReport<ArtistStatistics>>(() => new WishListBasedReport<ArtistStatistics>(context));
            _monthlySpend = new Lazy<IWishListBasedReport<MonthlySpend>>(() => new WishListBasedReport<MonthlySpend>(context));
        }
    }
}