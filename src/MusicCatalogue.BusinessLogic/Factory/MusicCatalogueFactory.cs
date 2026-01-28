using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.BusinessLogic.Database;
using MusicCatalogue.BusinessLogic.DataExchange.Catalogue;
using MusicCatalogue.BusinessLogic.DataExchange.Equipment;
using MusicCatalogue.BusinessLogic.Reporting;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.BusinessLogic.Factory
{
    public class MusicCatalogueFactory : IMusicCatalogueFactory
    {
        private readonly Lazy<IGenreManager> _genres;
        private readonly Lazy<IVibeManager> _vibes;
        private readonly Lazy<IArtistManager> _artists;
        private readonly Lazy<IAlbumManager> _albums;
        private readonly Lazy<ITrackManager> _tracks;
        private readonly Lazy<IManufacturerManager> _manufacturers;
        private readonly Lazy<IEquipmentTypeManager> _equipmentTypes;
        private readonly Lazy<IEquipmentManager> _equipment;
        private readonly Lazy<IRetailerManager> _retailers;
        private readonly Lazy<IUserManager> _users;
        private readonly Lazy<IImporter> _importer;
        private readonly Lazy<ITrackExporter> _catalogueCsvExporter;
        private readonly Lazy<ITrackExporter> _catalogueXlsxExporter;
        private readonly Lazy<IEquipmentExporter> _equipmentCsvExporter;
        private readonly Lazy<IEquipmentExporter> _equipmentXlsxExporter;
        private readonly Lazy<IJobStatusManager> _jobStatuses;
        private readonly Lazy<ISearchManager> _searchManager;
        private readonly Lazy<IWishListBasedReport<GenreStatistics>> _genreStatistics;
        private readonly Lazy<IWishListBasedReport<ArtistStatistics>> _artistStatistics;
        private readonly Lazy<IWishListBasedReport<MonthlySpend>> _monthlySpend;
        private readonly Lazy<IWishListBasedReport<RetailerStatistics>> _retailerStatistics;
        private readonly Lazy<IGenreBasedReport<GenreAlbum>> _genreAlbums;
        private readonly Lazy<IDateBasedReport<AlbumByPurchaseDate>> _albumsByPurchaseDate;

        public DbContext Context { get; private set; }
        public IGenreManager Genres { get { return _genres.Value; } }
        public IVibeManager Vibes { get { return _vibes.Value; } }
        public IArtistManager Artists { get { return _artists.Value; } }
        public IAlbumManager Albums { get { return _albums.Value; } }
        public ITrackManager Tracks { get { return _tracks.Value; } }
        public IManufacturerManager Manufacturers { get { return _manufacturers.Value; } }
        public IEquipmentTypeManager EquipmentTypes { get { return _equipmentTypes.Value; } }
        public IEquipmentManager Equipment { get { return _equipment.Value; } }
        public IRetailerManager Retailers { get { return _retailers.Value; } }
        public IJobStatusManager JobStatuses { get { return _jobStatuses.Value; } }
        public ISearchManager Search { get { return _searchManager.Value; } }
        public IUserManager Users { get { return _users.Value; } }
        public IImporter Importer { get {  return _importer.Value; } }
        public ITrackExporter CatalogueCsvExporter { get { return _catalogueCsvExporter.Value; } }
        public ITrackExporter CatalogueXlsxExporter { get { return _catalogueXlsxExporter.Value; } }
        public IEquipmentExporter EquipmentCsvExporter { get { return _equipmentCsvExporter.Value; } }
        public IEquipmentExporter EquipmentXlsxExporter { get { return _equipmentXlsxExporter.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<GenreStatistics> GenreStatistics { get { return _genreStatistics.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<ArtistStatistics> ArtistStatistics { get { return _artistStatistics.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<MonthlySpend> MonthlySpend { get { return _monthlySpend.Value; } }

        [ExcludeFromCodeCoverage]
        public IWishListBasedReport<RetailerStatistics> RetailerStatistics { get { return _retailerStatistics.Value; } }

        [ExcludeFromCodeCoverage]
        public IGenreBasedReport<GenreAlbum> GenreAlbums { get { return _genreAlbums.Value; } }

        [ExcludeFromCodeCoverage]
        public IDateBasedReport<AlbumByPurchaseDate> AlbumsByPurchaseDate { get { return _albumsByPurchaseDate.Value; } }

        public MusicCatalogueFactory(MusicCatalogueDbContext context)
        {
            Context = context;
            _genres = new Lazy<IGenreManager>(() => new GenreManager(this));
            _vibes = new Lazy<IVibeManager>(() => new VibeManager(this));
            _artists = new Lazy<IArtistManager>(() => new ArtistManager(this));
            _albums = new Lazy<IAlbumManager>(() => new AlbumManager(this));
            _tracks = new Lazy<ITrackManager>(() => new TrackManager(this));
            _manufacturers = new Lazy<IManufacturerManager>(() => new ManufacturerManager(this));
            _equipmentTypes = new Lazy<IEquipmentTypeManager>(() => new EquipmentTypeManager(this));
            _equipment = new Lazy<IEquipmentManager>(() => new EquipmentManager(this));
            _retailers = new Lazy<IRetailerManager>(() => new RetailerManager(this));
            _jobStatuses = new Lazy<IJobStatusManager>(() => new JobStatusManager(this));
            _searchManager = new Lazy<ISearchManager>(() => new SearchManager(this));
            _users = new Lazy<IUserManager>(() => new UserManager(this));
            _importer = new Lazy<IImporter>(() => new CatalogueCsvImporter(this));
            _catalogueCsvExporter = new Lazy<ITrackExporter>(() => new CatalogueCsvExporter(this));
            _catalogueXlsxExporter = new Lazy<ITrackExporter>(() => new CatalogueXlsxExporter(this));
            _equipmentCsvExporter = new Lazy<IEquipmentExporter>(() => new EquipmentCsvExporter(this));
            _equipmentXlsxExporter = new Lazy<IEquipmentExporter>(() => new EquipmentXlsxExporter(this));
            _genreStatistics = new Lazy<IWishListBasedReport<GenreStatistics>>(() => new WishListBasedReport<GenreStatistics>(context));
            _artistStatistics = new Lazy<IWishListBasedReport<ArtistStatistics>>(() => new WishListBasedReport<ArtistStatistics>(context));
            _monthlySpend = new Lazy<IWishListBasedReport<MonthlySpend>>(() => new WishListBasedReport<MonthlySpend>(context));
            _retailerStatistics = new Lazy<IWishListBasedReport<RetailerStatistics>>(() => new WishListBasedReport<RetailerStatistics>(context));
            _genreAlbums = new Lazy<IGenreBasedReport<GenreAlbum>>(() => new GenreBasedReport<GenreAlbum>(context));
            _albumsByPurchaseDate = new Lazy<IDateBasedReport<AlbumByPurchaseDate>>(() => new DateBasedReport<AlbumByPurchaseDate>(context));
        }
    }
}