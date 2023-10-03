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

        private IAlbumManager? _manager = null;
        private int _artistId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            var factory = new MusicCatalogueFactory(context);
            _manager = factory.Albums;

            // Add an artist to the database
            _artistId = Task.Run(() => factory.Artists.AddAsync(ArtistName)).Result.Id;
            Task.Run(() => _manager.AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl);
            var albums = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, albums.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var album = await _manager!.GetAsync(a => a.Title == AlbumTitle);
            Assert.IsNotNull(album);
            Assert.IsTrue(album.Id > 0);
            Assert.AreEqual(_artistId, album.ArtistId);
            Assert.AreEqual(AlbumTitle, album.Title);
            Assert.AreEqual(Released, album.Released);
            Assert.AreEqual(Genre, album.Genre);
            Assert.AreEqual(CoverUrl, album.CoverUrl);
            Assert.AreEqual(ArtistName, album.Artist!.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var album = await _manager!.GetAsync(a => a.Title == "Missing");
            Assert.IsNull(album);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var albums = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, albums!.Count);
            Assert.AreEqual(AlbumTitle, albums.First().Title);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var albums = await _manager!.ListAsync(e => e.Title == "Missing");
            Assert.AreEqual(0, albums!.Count);
        }
    }
}
