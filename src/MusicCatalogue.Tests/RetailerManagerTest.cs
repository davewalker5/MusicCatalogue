using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class RetailerManagerTest
    {
        private const string Name = "Dig Vinyl";

        private IRetailerManager? _manager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _manager = new MusicCatalogueFactory(context).Retailers;
            Task.Run(() => _manager.AddAsync(Name)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(Name);
            var retailers = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, retailers.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var retailer = await _manager!.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(retailer);
            Assert.IsTrue(retailer.Id > 0);
            Assert.AreEqual(Name, retailer.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var retailer = await _manager!.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(retailer);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var retailers = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, retailers!.Count);
            Assert.AreEqual(Name, retailers.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var retailers = await _manager!.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, retailers!.Count);
        }
    }
}
