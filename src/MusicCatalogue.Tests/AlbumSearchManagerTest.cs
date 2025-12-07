using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Search;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class AlbumSearchManagerTest
    {
        private const string JazzAlbumTitle = "Live In Paris";
        private const string PopAlbumTitle = "Album No. 8";

        private IMusicCatalogueFactory? _factory;
        private int _jazzGenreId;
        private int _popGenreId;
        private int _jazzArtistId;
        private int _popArtistId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Add the genres
            _jazzGenreId = Task.Run(() => _factory.Genres.AddAsync("Jazz", false)).Result.Id;
            _popGenreId = Task.Run(() => _factory.Genres.AddAsync("Pop", false)).Result.Id;

            // Add the artists
            _jazzArtistId = Task.Run(() => _factory.Artists.AddAsync("Diana Krall")).Result.Id;
            _popArtistId = Task.Run(() => _factory.Artists.AddAsync("Katie Melua")).Result.Id;

            // Add the albums, one on the wishlist and one not
            Task.Run(() => _factory.Albums.AddAsync(_jazzArtistId, _jazzGenreId, JazzAlbumTitle, 2002, null, false, null, null, null)).Wait();
            Task.Run(() => _factory.Albums.AddAsync(_popArtistId, _popGenreId, PopAlbumTitle, 2020, null, true, null, null, null)).Wait();
        }

        [TestMethod]
        public async Task SearchForAllAlbumsTest()
        {
            var albums = await _factory!.Search.AlbumSearchAsync(new AlbumSearchCriteria());
            Assert.IsNotNull(albums);
            Assert.AreEqual(2, albums.Count);
        }

        [TestMethod]
        public async Task SearchForMainCatalogueTest()
        {
            var criteria = new AlbumSearchCriteria { WishList = false };
            var albums = await _factory!.Search.AlbumSearchAsync(criteria);
            Assert.IsNotNull(albums);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(JazzAlbumTitle, albums[0].Title);
        }

        [TestMethod]
        public async Task SearchForWishListTest()
        {
            var criteria = new AlbumSearchCriteria { WishList = true };
            var albums = await _factory!.Search.AlbumSearchAsync(criteria);
            Assert.IsNotNull(albums);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(PopAlbumTitle, albums[0].Title);
        }

        [TestMethod]
        public async Task SearchForArtistTest()
        {
            var criteria = new AlbumSearchCriteria { ArtistId = _popArtistId };
            var albums = await _factory!.Search.AlbumSearchAsync(criteria);
            Assert.IsNotNull(albums);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(PopAlbumTitle, albums[0].Title);
        }

        [TestMethod]
        public async Task SearchForGenreTest()
        {
            var criteria = new AlbumSearchCriteria { GenreId = _jazzGenreId };
            var albums = await _factory!.Search.AlbumSearchAsync(criteria);
            Assert.IsNotNull(albums);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(JazzAlbumTitle, albums[0].Title);
        }

        [TestMethod]
        public async Task SearchForCombinationTest()
        {
            var criteria = new AlbumSearchCriteria
            {
                ArtistId = _popArtistId,
                GenreId = _popGenreId,
                WishList = true
            };
            var albums = await _factory!.Search.AlbumSearchAsync(criteria);
            Assert.IsNotNull(albums);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(PopAlbumTitle, albums[0].Title);
        }

        [TestMethod]
        public async Task NoMatchesTest()
        {
            var criteria = new AlbumSearchCriteria
            {
                ArtistId = _popArtistId,
                GenreId = _popGenreId,
                WishList = false
            };
            var albums = await _factory!.Search.AlbumSearchAsync(criteria);
            Assert.IsNull(albums);
        }
    }
}
