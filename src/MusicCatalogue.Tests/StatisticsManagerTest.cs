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
        private DateTime Purchased = new(2023, 11, 1);
        private const decimal Price = 37.99M;
        private const string RetailerName = "Truck Store";
        private const string TrackTitle = "Blue Train";
        private const int TrackNumber = 1;
        private const int TrackDuration = 643200;

        private IMusicCatalogueFactory? _factory = null;
        private int _retailerId;
        private int _artistId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Set up an artist and album for the tracks to belong to and add a track
            _retailerId = Task.Run(() => _factory.Retailers.AddAsync(RetailerName)).Result.Id;
            _artistId = Task.Run(() => _factory.Artists.AddAsync(ArtistName)).Result.Id;
        }

        [TestMethod]
        public void ArtistStatisticsForMainCatalogueTest()
        {
            // Add an album to the main catalogue
            var albumId = Task.Run(() => _factory!.Albums.AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl, false, Purchased, Price, _retailerId)).Result.Id;
            Task.Run(() => _factory!.Tracks.AddAsync(albumId, TrackTitle, TrackNumber, TrackDuration)).Wait();

            var artists = Task.Run(() => _factory!.Artists.ListAsync(x => true)).Result;
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.IsNull(artists[0].AlbumCount);
            Assert.IsNull(artists[0].TrackCount);
            Assert.AreEqual(0M, artists[0].TotalAlbumSpend);

            Task.Run(() => _factory!.Statistics.PopulateArtistStatistics(artists, false)).Wait();
            Assert.AreEqual(1, artists[0].AlbumCount);
            Assert.AreEqual(1, artists[0].TrackCount);
            Assert.AreEqual(Price, artists[0].TotalAlbumSpend);
        }

        [TestMethod]
        public void ArtistStatisticsForWishListTest()
        {
            // Add an album to the wish list
            var albumId = Task.Run(() => _factory!.Albums.AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl, true, Purchased, Price, _retailerId)).Result.Id;
            Task.Run(() => _factory!.Tracks.AddAsync(albumId, TrackTitle, TrackNumber, TrackDuration)).Wait();

            var artists = Task.Run(() => _factory!.Artists.ListAsync(x => true)).Result;
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.IsNull(artists[0].AlbumCount);
            Assert.IsNull(artists[0].TrackCount);
            Assert.AreEqual(0M, artists[0].TotalAlbumSpend);

            Task.Run(() => _factory!.Statistics.PopulateArtistStatistics(artists, true)).Wait();
            Assert.AreEqual(1, artists[0].AlbumCount);
            Assert.AreEqual(1, artists[0].TrackCount);
            Assert.AreEqual(Price, artists[0].TotalAlbumSpend);
        }
    }
}
