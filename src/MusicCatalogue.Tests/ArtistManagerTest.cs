using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Database;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ArtistManagerTest
    {
        private const string Name = "Diana Krall";

        private IArtistManager? _manager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _manager = new ArtistManager(context);
            Task.Run(() => _manager.AddAsync(Name)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(Name);
            var artists = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, artists.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var artist = await _manager!.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(artist);
            Assert.IsTrue(artist.Id > 0);
            Assert.AreEqual(Name, artist.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var artist = await _manager!.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(artist);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var artists = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual(Name, artists.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var artists = await _manager!.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, artists!.Count);
        }
    }
}
