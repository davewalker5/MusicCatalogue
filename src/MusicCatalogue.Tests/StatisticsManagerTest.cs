using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class StatisticsManagerTest
    {
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "https://some.host/blue-train.jpg";
        private const string TrackTitle = "Blue Train";
        private const int TrackNumber = 1;
        private const int TrackDuration = 643200;

        private IMusicCatalogueFactory? _factory = null;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Set up an artist and album for the tracks to belong to and add a track
            var artistId = Task.Run(() => _factory.Artists.AddAsync(ArtistName)).Result.Id;
            var albumId = Task.Run(() => _factory.Albums.AddAsync(artistId, AlbumTitle, Released, Genre, CoverUrl, false)).Result.Id;
            Task.Run(() => _factory.Tracks.AddAsync(albumId, TrackTitle, TrackNumber, TrackDuration)).Wait();
        }

        [TestMethod]
        public void ArtistStatisticsTest()
        {
            var artists = Task.Run(() => _factory!.Artists.ListAsync(x => true)).Result;
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.IsNull(artists[0].AlbumCount);
            Assert.IsNull(artists[0].TrackCount);

            Task.Run(() => _factory!.Statistics.PopulateArtistStatistics(artists, false)).Wait();
            Assert.AreEqual(1, artists[0].AlbumCount);
            Assert.AreEqual(1, artists[0].TrackCount);
        }
    }
}
