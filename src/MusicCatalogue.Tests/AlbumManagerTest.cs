using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class AlbumManagerTest
    {
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "https://some.host/blue-train.jpg";
        private const string TrackTitle = "Blue Train";
        private const int TrackNumber = 1;
        private const int TrackDuration = 643200;
        private readonly DateTime Purchased = new(2023, 11, 1);
        private const decimal Price = 37.99M;
        private const string RetailerName = "Truck Store";

        private IMusicCatalogueFactory? _factory;
        private int _retailerId;
        private int _genreId;
        private int _artistId;
        private int _albumId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context, new MockFileLogger());

            // Add the test entities to the database
            _retailerId = Task.Run(() => _factory.Retailers.AddAsync(RetailerName)).Result.Id;
            _genreId = Task.Run(() => _factory.Genres.AddAsync(Genre, false)).Result.Id;
            _artistId = Task.Run(() => _factory.Artists.AddAsync(ArtistName)).Result.Id;
            _albumId = Task.Run(() => _factory.Albums.AddAsync(_artistId, _genreId, AlbumTitle, Released, CoverUrl, false, null, null, null)).Result.Id;
            Task.Run(() => _factory.Tracks.AddAsync(_albumId, TrackTitle, TrackNumber, TrackDuration)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _factory!.Albums.AddAsync(_artistId, _genreId, AlbumTitle, Released, CoverUrl, false, null, null, null);
            var albums = await _factory!.Albums.ListAsync(x => true);
            Assert.AreEqual(1, albums.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var album = await _factory!.Albums.GetAsync(a => a.Title == AlbumTitle);
            Assert.IsNotNull(album);
            Assert.IsTrue(album.Id > 0);
            Assert.AreEqual(_artistId, album.ArtistId);
            Assert.AreEqual(AlbumTitle, album.Title);
            Assert.AreEqual(Released, album.Released);
            Assert.AreEqual(Genre, album.Genre!.Name);
            Assert.AreEqual(CoverUrl, album.CoverUrl);
            Assert.IsFalse(album.IsWishListItem);
            Assert.IsNull(album.Purchased);
            Assert.IsNull(album.Price);
            Assert.IsNull(album.RetailerId);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var album = await _factory!.Albums.UpdateAsync(_albumId, _artistId, _genreId, AlbumTitle, Released, CoverUrl, true, Purchased, Price, _retailerId);
            Assert.IsNotNull(album);
            Assert.IsTrue(album.Id > 0);
            Assert.AreEqual(_artistId, album.ArtistId);
            Assert.AreEqual(AlbumTitle, album.Title);
            Assert.AreEqual(Released, album.Released);
            Assert.AreEqual(Genre, album.Genre!.Name);
            Assert.AreEqual(CoverUrl, album.CoverUrl);
            Assert.IsTrue(album.IsWishListItem);
            Assert.AreEqual(Purchased, album.Purchased);
            Assert.AreEqual(Price, album.Price);
            Assert.AreEqual(_retailerId, album.RetailerId);
            Assert.AreEqual(RetailerName, album.Retailer!.Name);
        }

        [TestMethod]
        public async Task UpdateMissingTest()
        {
            var album = await _factory!.Albums.UpdateAsync(-1, _artistId, _genreId, AlbumTitle, Released, CoverUrl, true, null, null, null);
            Assert.IsNull(album);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var album = await _factory!.Albums.GetAsync(a => a.Title == "Missing");
            Assert.IsNull(album);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var albums = await _factory!.Albums.ListAsync(x => true);
            Assert.AreEqual(1, albums!.Count);
            Assert.AreEqual(AlbumTitle, albums.First().Title);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var albums = await _factory!.Albums.ListAsync(e => e.Title == "Missing");
            Assert.AreEqual(0, albums!.Count);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            var album = await _factory!.Albums.GetAsync(a => a.Title == AlbumTitle);
            Assert.IsNotNull(album);
            Assert.AreEqual(1, album.Tracks!.Count);

            await _factory!.Albums.DeleteAsync(_albumId);
            album = await _factory.Albums!.GetAsync(a => a.Title == AlbumTitle);
            Assert.IsNull(album);

            var tracks = await _factory.Tracks.ListAsync(x => x.AlbumId == _albumId);
            Assert.IsFalse(tracks.Any());
        }
    }
}
