using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class VibeManagerTest
    {
        private const string Name = "Late Night";
        private const string UpdatedName = "Upbeat";

        private IMusicCatalogueFactory? _factory = null;
        private int _vibeId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            _vibeId = Task.Run(() => _factory!.Vibes.AddAsync(Name)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            var vibes = await _factory!.Vibes.ListAsync(x => true);
            Assert.AreEqual(1, vibes.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var vibe = await _factory!.Vibes.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(vibe);
            Assert.IsTrue(vibe.Id > 0);
            Assert.AreEqual(Name, vibe.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var vibe = await _factory!.Vibes.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(vibe);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var vibes = await _factory!.Vibes.ListAsync(x => true);
            Assert.AreEqual(1, vibes!.Count);
            Assert.AreEqual(Name, vibes.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var vibes = await _factory!.Vibes.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, vibes!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _factory!.Vibes.UpdateAsync(_vibeId, UpdatedName);
            var vibe = await _factory!.Vibes.GetAsync(a => a.Id == _vibeId);
            Assert.IsNotNull(vibe);
            Assert.IsTrue(vibe.Id > 0);
            Assert.AreEqual(UpdatedName, vibe.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Vibes.DeleteAsync(_vibeId);
            var vibes = await _factory!.Vibes.ListAsync(a => true);
            Assert.AreEqual(0, vibes!.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(VibeInUseException))]
        public async Task CannotDeleteWithEquipmentTest()
        {
            var context = _factory!.Context as MusicCatalogueDbContext;
            var artist = new Artist { Name = "Julie London", VibeId = _vibeId };
            await context!.Artists.AddAsync(artist);
            await context!.SaveChangesAsync();
            await _factory!.Vibes.DeleteAsync(_vibeId);
        }
    }
}