using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ArtistManagerTest
    {
        private const string Name = "The Beatles";

        private IArtistManager? _manager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _manager = new MusicCatalogueFactory(context).Artists;
            Task.Run(() => _manager!.AddAsync(Name)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            var artists = await _manager!.ListAsync(x => true, false);
            Assert.AreEqual(1, artists.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var artist = await _manager!.GetAsync(a => a.Name == Name, false);
            Assert.IsNotNull(artist);
            Assert.IsTrue(artist.Id > 0);
            Assert.AreEqual(Name, artist.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var artist = await _manager!.GetAsync(a => a.Name == "Missing", false);
            Assert.IsNull(artist);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var artists = await _manager!.ListAsync(x => true, false);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual(Name, artists.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var artists = await _manager!.ListAsync(e => e.Name == "Missing", false);
            Assert.AreEqual(0, artists!.Count);
        }

        [TestMethod]
        public async Task ListBySearchableNameTest()
        {
            var artists = await _manager!.ListAsync(x => (x.SearchableName ?? x.Name).StartsWith("B"), false);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual("The Beatles", artists[0].Name);

        }
    }
}
