using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

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

        private IMusicCatalogueFactory? _factory;
        private int _artistId;
        private int _albumId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Add an artist to the database
            _artistId = Task.Run(() => _factory.Artists.AddAsync(ArtistName)).Result.Id;
            _albumId = Task.Run(() => _factory.Albums.AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl)).Result.Id;
            Task.Run(() => _factory.Tracks.AddAsync(_albumId, TrackTitle, TrackNumber, TrackDuration)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _factory!.Albums.AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl);
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
            Assert.AreEqual(Genre, album.Genre);
            Assert.AreEqual(CoverUrl, album.CoverUrl);
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
            Assert.AreEqual(1, album.Tracks.Count);

            await _factory!.Albums.DeleteAsync(_albumId);
            album = await _factory.Albums!.GetAsync(a => a.Title == AlbumTitle);
            Assert.IsNull(album);

            var tracks = await _factory.Tracks.ListAsync(x => x.AlbumId == _albumId);
            Assert.IsFalse(tracks.Any());
        }
    }
}
