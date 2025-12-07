using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Search;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class GenreSearchManagerTest
    {
        private const string JazzGenre = "Jazz";
        private const string PopGenre = "Pop";

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
        }

        [TestMethod]
        public async Task SearchForAllGenresTest()
        {
            var genres = await _factory!.Search.GenreSearchAsync(new GenreSearchCriteria());
            Assert.IsNotNull(genres);
            Assert.AreEqual(2, genres.Count);
        }

        [TestMethod]
        public async Task SearchMainCatalogueGenresTest()
        {
            // Add the albums, one on the wishlist and one not
            Task.Run(() => _factory!.Albums.AddAsync(_jazzArtistId, _jazzGenreId, "Live In Paris", 2002, null, false, null, null, null)).Wait();
            Task.Run(() => _factory!.Albums.AddAsync(_popArtistId, _popGenreId, "Album No. 8", 2020, null, true, null, null, null)).Wait();

            var criteria = new GenreSearchCriteria { WishList = false };
            var genres = await _factory!.Search.GenreSearchAsync(criteria);
            Assert.IsNotNull(genres);
            Assert.AreEqual(1, genres.Count);
            Assert.AreEqual(JazzGenre, genres[0].Name);
        }

        [TestMethod]
        public async Task SearchWishlistGenresTest()
        {
            // Add the albums, one on the wishlist and one not
            Task.Run(() => _factory!.Albums.AddAsync(_jazzArtistId, _jazzGenreId, "Live In Paris", 2002, null, false, null, null, null)).Wait();
            Task.Run(() => _factory!.Albums.AddAsync(_popArtistId, _popGenreId, "Album No. 8", 2020, null, true, null, null, null)).Wait();

            var criteria = new GenreSearchCriteria { WishList = true };
            var genres = await _factory!.Search.GenreSearchAsync(criteria);
            Assert.IsNotNull(genres);
            Assert.AreEqual(1, genres.Count);
            Assert.AreEqual(PopGenre, genres[0].Name);
        }
    }
}
