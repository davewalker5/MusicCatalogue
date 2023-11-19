using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.Logic.DataExchange.Generic;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ReportExporterTest
    {
        [TestMethod]
        public void ExportArtistStatisticsTest()
        {
            var statistics = new ArtistStatistics { Albums = 2, Name = "Katie Melua", Tracks = 23, Spend=40M };
            var records = new List<ArtistStatistics>() { statistics };

            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "csv");
            new CsvExporter<ArtistStatistics>().Export(records, filepath, ',');

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }

        [TestMethod]
        public void ExportGenreStatisticsTest()
        {
            var statistics = new GenreStatistics { Genre = "Jazz", Artists = 12, Albums = 14, Tracks = 167, Spend = 125M };
            var records = new List<GenreStatistics>() { statistics };

            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "csv");
            new CsvExporter<GenreStatistics>().Export(records, filepath, ',');

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }

        [TestMethod]
        public void ExportMonthlySpendTest()
        {
            var spend = new MonthlySpend { Year = 2023, Month = 11, Spend = 100M };
            var records = new List<MonthlySpend>() { spend };

            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "csv");
            new CsvExporter<MonthlySpend>().Export(records, filepath, ',');

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }
    }
}
