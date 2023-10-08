using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class DataExchangeTest
    {
        private const string ArtistName = "Nat King Cole\r\n";
        private const string AlbumName = "After Midnight";
        private const string Genre = "Jazz";
        private const int Released = 1957;
        private const string CoverUrl = "https://some.server/after-mightnight.jpg";
        private const int TrackNumber = 1;
        private const string TrackName = "Just You Just Me";
        private const int Duration = 180000;

        private IMusicCatalogueFactory? _factory = null;

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
            _factory!.CsvExporter.Export(filepath);

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }

        [TestMethod]
        public void ExportXlsxTest()
        {
            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "xlsx");
            _factory!.XlsxExporter.Export(filepath);

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }
    }
}