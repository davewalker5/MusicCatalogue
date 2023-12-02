using MusicCatalogue.Data;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ArtistManagerTest
    {
        private const string ArtistName = "The Beatles";
        private const string AlbumTitle = "Rubber Soul";
        private const int Released = 1965;
        private const string Genre = "Pop";

        private const string UpdatedArtistName = "Katie Melua";

        private IMusicCatalogueFactory? _factory;
        private int _artistId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);
            _artistId = Task.Run(() => _factory.Artists.AddAsync(ArtistName)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            var artists = await _factory!.Artists.ListAsync(x => true, false);
            Assert.AreEqual(1, artists.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var artist = await _factory!.Artists.GetAsync(a => a.Name == ArtistName, false);
            Assert.IsNotNull(artist);
            Assert.IsTrue(artist.Id > 0);
            Assert.AreEqual(ArtistName, artist.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var artist = await _factory!.Artists.GetAsync(a => a.Name == "Missing", false);
            Assert.IsNull(artist);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var artists = await _factory!.Artists.ListAsync(x => true, false);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual(ArtistName, artists.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var artists = await _factory!.Artists.ListAsync(e => e.Name == "Missing", false);
            Assert.AreEqual(0, artists!.Count);
        }

        [TestMethod]
        public async Task ListBySearchableNameTest()
        {
            var artists = await _factory!.Artists.ListAsync(x => (x.SearchableName ?? x.Name).StartsWith("B"), false);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual("The Beatles", artists[0].Name);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _factory!.Artists.UpdateAsync(_artistId, UpdatedArtistName);
            var artist = await _factory!.Artists.GetAsync(a => a.Id == _artistId, false);
            Assert.IsNotNull(artist);
            Assert.IsTrue(artist.Id > 0);
            Assert.AreEqual(UpdatedArtistName, artist.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Artists.DeleteAsync(_artistId);
            var artists = await _factory!.Artists.ListAsync(a => true, false);
            Assert.AreEqual(0, artists!.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArtistInUseException))]
        public async Task CannotDeleteWithAlbumTest()
        {
            var genreId = Task.Run(() => _factory!.Genres.AddAsync(Genre)).Result.Id;
            Task.Run(() => _factory!.Albums.AddAsync(_artistId, genreId, AlbumTitle, Released, null, false, null, null, null)).Wait();
            await _factory!.Artists.DeleteAsync(_artistId);
        }
    }
}
