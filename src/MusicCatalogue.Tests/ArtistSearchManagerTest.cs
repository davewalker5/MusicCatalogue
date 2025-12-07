using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Search;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ArtistSearchManagerTest
    {
        private const string JazzArtistName = "Diana Krall";
        private const string PopArtistName = "Katie Melua";
        private const string NoAlbumsArtistName = "Fleetwood Mac";

        private const string JazzAlbumTitle = "Live In Paris";
        private const string PopAlbumTitle = "Album No. 8";

        private IMusicCatalogueFactory? _factory;
        private int _jazzGenreId;
        private int _popGenreId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Add the genres
            _jazzGenreId = Task.Run(() => _factory.Genres.AddAsync("Jazz", false)).Result.Id;
            _popGenreId = Task.Run(() => _factory.Genres.AddAsync("Pop", false)).Result.Id;

            // Add the artists
            var jazzArtistId = Task.Run(() => _factory.Artists.AddAsync(JazzArtistName)).Result.Id;
            var popArtistId = Task.Run(() => _factory.Artists.AddAsync(PopArtistName)).Result.Id;
            Task.Run(() => _factory.Artists.AddAsync(NoAlbumsArtistName)).Wait();

            // Add the albums, one on the wishlist and one not
            Task.Run(() => _factory.Albums.AddAsync(jazzArtistId, _jazzGenreId, JazzAlbumTitle, 2002, null, false, null, null, null)).Wait();
            Task.Run(() => _factory.Albums.AddAsync(popArtistId, _popGenreId, PopAlbumTitle, 2020, null, true, null, null, null)).Wait();
        }

        [TestMethod]
        public async Task SearchForAllArtistsTest()
        {
            var artists = await _factory!.Search.ArtistSearchAsync(new ArtistSearchCriteria());
            Assert.IsNotNull(artists);
            Assert.AreEqual(2, artists.Count);

            var jazzArtist = artists.First(x => x.Name == JazzArtistName);
            Assert.IsNotNull(jazzArtist.Albums);
            Assert.AreEqual(1, jazzArtist.Albums.Count);
            Assert.AreEqual(JazzAlbumTitle, jazzArtist.Albums.First().Title);

            var popArtist = artists.First(x => x.Name == PopArtistName);
            Assert.IsNotNull(popArtist.Albums);
            Assert.AreEqual(1, popArtist.Albums.Count);
            Assert.AreEqual(PopAlbumTitle, popArtist.Albums.First().Title);
        }

        [TestMethod]
        public async Task SearchForAllArtistsWithWildcardTest()
        {
            var criteria = new ArtistSearchCriteria { NamePrefix = "*" };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(2, artists.Count);

            var jazzArtist = artists.First(x => x.Name == JazzArtistName);
            Assert.IsNotNull(jazzArtist.Albums);
            Assert.AreEqual(1, jazzArtist.Albums.Count);
            Assert.AreEqual(JazzAlbumTitle, jazzArtist.Albums.First().Title);

            var popArtist = artists.First(x => x.Name == PopArtistName);
            Assert.IsNotNull(popArtist.Albums);
            Assert.AreEqual(1, popArtist.Albums.Count);
            Assert.AreEqual(PopAlbumTitle, popArtist.Albums.First().Title);
        }

        [TestMethod]
        public async Task SearchForMainCatalogueTest()
        {
            var criteria = new ArtistSearchCriteria { WishList = false };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.AreEqual(JazzArtistName, artists[0].Name);
            Assert.AreEqual(JazzAlbumTitle, artists[0].Albums!.First().Title);
        }

        [TestMethod]
        public async Task SearchForWishListTest()
        {
            var criteria = new ArtistSearchCriteria { WishList = true };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.AreEqual(PopArtistName, artists[0].Name);
            Assert.AreEqual(PopAlbumTitle, artists[0].Albums!.First().Title);
        }

        [TestMethod]
        public async Task SearchForArtistByNamePrefixTest()
        {
            var criteria = new ArtistSearchCriteria { NamePrefix = PopArtistName[..1] };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.AreEqual(PopArtistName, artists[0].Name);
            Assert.AreEqual(PopAlbumTitle, artists[0].Albums!.First().Title);
        }

        [TestMethod]
        public async Task SearchForGenreTest()
        {
            var criteria = new ArtistSearchCriteria { GenreId = _jazzGenreId };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.AreEqual(JazzArtistName, artists[0].Name);
            Assert.AreEqual(JazzAlbumTitle, artists[0].Albums!.First().Title);
        }

        [TestMethod]
        public async Task SearchForCombinationTest()
        {
            var criteria = new ArtistSearchCriteria
            {
                NamePrefix = PopArtistName[..1],
                GenreId = _popGenreId,
                WishList = true
            };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(1, artists.Count);
            Assert.AreEqual(PopArtistName, artists[0].Name);
            Assert.AreEqual(PopAlbumTitle, artists[0].Albums!.First().Title);
        }

        [TestMethod]
        public async Task NoMatchesTest()
        {
            var criteria = new ArtistSearchCriteria
            {
                NamePrefix = PopArtistName[..1],
                GenreId = _popGenreId,
                WishList = false
            };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNull(artists);
        }

        [TestMethod]
        public async Task IncludeArtistsWithNoAlbumsTest()
        {
            var criteria = new ArtistSearchCriteria
            {
                IncludeArtistsWithNoAlbums = true
            };
            var artists = await _factory!.Search.ArtistSearchAsync(criteria);
            Assert.IsNotNull(artists);
            Assert.AreEqual(3, artists.Count);
        }
    }
}
