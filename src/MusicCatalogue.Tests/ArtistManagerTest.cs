using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

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
            _manager = new MusicCatalogueFactory(context).Artists;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(Name);
            await _manager!.AddAsync(Name);
            var artists = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, artists.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            await _manager!.AddAsync(Name);
            var artist = await _manager!.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(artist);
            Assert.IsTrue(artist.Id > 0);
            Assert.AreEqual(Name, artist.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            await _manager!.AddAsync(Name);
            var artist = await _manager!.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(artist);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            await _manager!.AddAsync(Name);
            var artists = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual(Name, artists.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            await _manager!.AddAsync(Name);
            var artists = await _manager!.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, artists!.Count);
        }

        [TestMethod]
        public async Task ListByNameTest()
        {
            await _manager!.AddAsync(Name);
            var artists = await _manager!.ListByNameAsync(Name[..1]);
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual(Name, artists[0].Name);
        }

        [TestMethod]
        public async Task ListBySearchableNameTest()
        {
            await _manager!.AddAsync("The Beatles");
            var artists = await _manager!.ListByNameAsync("B");
            Assert.AreEqual(1, artists!.Count);
            Assert.AreEqual("The Beatles", artists[0].Name);

        }
    }
}
