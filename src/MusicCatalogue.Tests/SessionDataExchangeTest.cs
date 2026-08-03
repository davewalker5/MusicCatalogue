using ClosedXML.Excel;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class SessionDataExchangeTest
    {
        private IMusicCatalogueFactory? _factory;
        private Session? _session;

        [TestInitialize]
        public async Task Initialize()
        {
            var context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context, new MockFileLogger());

            var artist = await _factory.Artists.AddAsync("Miles Davis");
            var genre = await _factory.Genres.AddAsync("Jazz", false);
            var first = await _factory.Albums.AddAsync(artist.Id, genre.Id, "Kind of Blue", 1959, null, false, null, null, null);
            var second = await _factory.Albums.AddAsync(artist.Id, genre.Id, "In a Silent Way", 1969, null, false, null, null, null);
            await _factory.Tracks.AddAsync(first.Id, "So What", 1, 545000);
            await _factory.Tracks.AddAsync(second.Id, "Shhh", 1, 1080000);
            _session = await _factory.SessionManager.AddAsync(
                new DateTime(2026, 8, 3), PlaylistType.Curated, TimeOfDay.Evening, [first.Id, second.Id]);
        }

        [TestMethod]
        public void CsvExportWritesHeadersAlbumsTotalAndRaisesEvents()
        {
            var path = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            var exported = new List<string>();
            _factory!.SessionCsvExporter.SessionAlbumExport += (_, e) =>
                exported.Add($"{e.RecordCount}:{e.Item!.AlbumTitle}");

            try
            {
                _factory.SessionCsvExporter.Export(path, _session!);
                var lines = File.ReadAllLines(path);

                Assert.AreEqual("#,Artist,Album,Playing Time", lines[0].TrimStart('\uFEFF'));
                Assert.AreEqual("2,Miles Davis,Kind Of Blue,00:09:05", lines[1]);
                Assert.AreEqual("3,Miles Davis,In A Silent Way,00:18:00", lines[2]);
                Assert.AreEqual(",,,00:27:05", lines[3]);
                CollectionAssert.AreEqual(
                    new[] { "1:Kind Of Blue", "2:In A Silent Way" }, exported);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void XlsxExportWritesHeadersAlbumsAndTotal()
        {
            var path = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx");
            var exportedCount = 0;
            _factory!.SessionXlsxExporter.SessionAlbumExport += (_, _) => exportedCount++;

            try
            {
                _factory.SessionXlsxExporter.Export(path, _session!);

                using var workbook = new XLWorkbook(path);
                var sheet = workbook.Worksheet("Session");
                Assert.AreEqual("#", sheet.Cell(1, 1).GetString());
                Assert.AreEqual("Artist", sheet.Cell(1, 2).GetString());
                Assert.AreEqual("Album", sheet.Cell(1, 3).GetString());
                Assert.AreEqual("Playing Time", sheet.Cell(1, 4).GetString());
                Assert.AreEqual("2", sheet.Cell(2, 1).GetString());
                Assert.AreEqual("Miles Davis", sheet.Cell(2, 2).GetString());
                Assert.AreEqual("Kind Of Blue", sheet.Cell(2, 3).GetString());
                Assert.AreEqual("00:09:05", sheet.Cell(2, 4).GetString());
                Assert.AreEqual("00:27:05", sheet.Cell(4, 4).GetString());
                Assert.AreEqual(2, exportedCount);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public async Task EmptySessionCanBeExported()
        {
            var empty = await _factory!.SessionManager.AddAsync(
                DateTime.UtcNow, PlaylistType.Normal, TimeOfDay.Morning, []);
            var path = Path.ChangeExtension(Path.GetTempFileName(), ".csv");

            try
            {
                _factory.SessionCsvExporter.Export(path, empty);
                var lines = File.ReadAllLines(path);

                Assert.AreEqual(2, lines.Length);
                Assert.AreEqual(",,,00:00:00", lines[1]);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
