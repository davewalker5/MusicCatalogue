using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Database;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class DataExchangeTest
    {
        private const string ArtistName = "Nat, King Cole\r\n";
        private const string AlbumName = "After Midnight";
        private const string Genre = "Jazz";
        private const int Released = 1957;
        private const string CoverUrl = "https://some.server/after-mightnight.jpg";
        private const int TrackNumber = 1;
        private const string TrackName = "Just You Just Me";
        private const int Duration = 180000;

        private IMusicCatalogueFactory? _factory = null;
        private string _cleanArtistName = StringCleaner.Clean(ArtistName)!;

        [TestInitialize]
        public void Initialise()
        {
            // Create an instance of the factory
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Add an artist, an album and one track
            var artist = Task.Run(() => _factory.Artists.AddAsync(ArtistName)).Result;
            var album = Task.Run(() => _factory.Albums.AddAsync(artist.Id, AlbumName, Released, Genre, CoverUrl)).Result;
            Task.Run(() => _factory.Tracks.AddAsync(album.Id, TrackName, TrackNumber, Duration)).Wait();
        }

        [TestMethod]
        public void ExportCsvTest()
        {
            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "csv");
            Task.Run(() => _factory!.CsvExporter.Export(filepath)).Wait();

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }

        [TestMethod]
        public void ExportXlsxTest()
        {
            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "xlsx");
            Task.Run(() => _factory!.XlsxExporter.Export(filepath)).Wait();

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }

        [TestMethod]
        public void ImportCsvTest()
        {
            // Export the data
            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "csv");
            Task.Run(() => _factory!.CsvExporter.Export(filepath)).Wait();

            // Create a new instance of the factory with a new in-memory database
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            var factory = new MusicCatalogueFactory(context);

            // Confirm the data's not there
            var artists = Task.Run(() => factory.Artists.ListAsync(x => true)).Result;
            var albums = Task.Run(() => factory.Albums.ListAsync(x => true)).Result;
            var tracks = Task.Run(() => factory.Tracks.ListAsync(x => true)).Result;

            Assert.AreEqual(0, artists.Count);
            Assert.AreEqual(0, albums.Count);
            Assert.AreEqual(0, tracks.Count);

            // Import the data
            Task.Run(() => factory.Importer.Import(filepath)).Wait();
            artists = Task.Run(() => factory.Artists.ListAsync(x => true)).Result;
            albums = Task.Run(() => factory.Albums.ListAsync(x => true)).Result;
            tracks = Task.Run(() => factory.Tracks.ListAsync(x => true)).Result;

            Assert.AreEqual(1, artists.Count);
            Assert.AreEqual(_cleanArtistName, artists[0].Name);

            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(AlbumName, albums[0].Title);
            Assert.AreEqual(Genre, albums[0].Genre);
            Assert.AreEqual(Released, albums[0].Released);
            Assert.AreEqual(CoverUrl, albums[0].CoverUrl);

            Assert.AreEqual(1, tracks.Count);
            Assert.AreEqual(TrackNumber, tracks[0].Number);
            Assert.AreEqual(TrackName, tracks[0].Title);
            Assert.AreEqual(Duration, tracks[0].Duration);
        }
    }
}