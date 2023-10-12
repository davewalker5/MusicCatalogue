using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Database;
using MusicCatalogue.Logic.DataExchange;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.Factory
{
    public class MusicCatalogueFactory : IMusicCatalogueFactory
    {
        private readonly Lazy<IArtistManager> _artists;
        private readonly Lazy<IAlbumManager> _albums;
        private readonly Lazy<ITrackManager> _tracks;
        private readonly Lazy<IUserManager> _users;
        private readonly Lazy<IImporter> _importer;
        private readonly Lazy<IExporter> _csvExporter;
        private readonly Lazy<IExporter> _xlsxExporter;
        private readonly Lazy<IStatisticsManager> _statistics;

        public IArtistManager Artists { get { return _artists.Value; } }
        public IAlbumManager Albums { get { return _albums.Value; } }
        public ITrackManager Tracks { get { return _tracks.Value; } }
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
            _artists = new Lazy<IArtistManager>(() => new ArtistManager(context));
            _albums = new Lazy<IAlbumManager>(() => new AlbumManager(context));
            _tracks = new Lazy<ITrackManager>(() => new TrackManager(context));
            _users = new Lazy<IUserManager>(() => new UserManager(context));
            _importer = new Lazy<IImporter>(() => new CsvImporter(this));
            _csvExporter = new Lazy<IExporter>(() => new CsvExporter(this));
            _xlsxExporter = new Lazy<IExporter>(() => new XlsxExporter(this));
            _statistics = new Lazy<IStatisticsManager>(() => new StatisticsManager(this));
        }
    }
}