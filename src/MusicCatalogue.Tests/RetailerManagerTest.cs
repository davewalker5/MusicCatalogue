using MusicCatalogue.Data;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class RetailerManagerTest
    {
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "https://some.host/blue-train.jpg";
        private const string Name = "Dig Vinyl";
        private const string UpdatedName = "Truck Store";

        private IMusicCatalogueFactory? _factory = null;
        private int _retailerId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);
            _retailerId = Task.Run(() => _factory.Retailers.AddAsync(Name)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _factory!.Retailers.AddAsync(Name);
            var retailers = await _factory.Retailers.ListAsync(x => true);
            Assert.AreEqual(1, retailers.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var retailer = await _factory!.Retailers.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(retailer);
            Assert.IsTrue(retailer.Id > 0);
            Assert.AreEqual(Name, retailer.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var retailer = await _factory!.Retailers.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(retailer);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var retailers = await _factory!.Retailers.ListAsync(x => true);
            Assert.AreEqual(1, retailers!.Count);
            Assert.AreEqual(Name, retailers.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var retailers = await _factory!.Retailers.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, retailers!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var retailer = await _factory!.Retailers.UpdateAsync(_retailerId, UpdatedName);
            Assert.IsNotNull(retailer);
            Assert.IsTrue(retailer.Id > 0);
            Assert.AreEqual(UpdatedName, retailer.Name);
        }

        [TestMethod]
        public async Task UpdateMissingTest()
        {
            var retailer = await _factory!.Retailers.UpdateAsync(-1, UpdatedName);
            Assert.IsNull(retailer);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Retailers.DeleteAsync(_retailerId);
            var retailers = await _factory!.Retailers.ListAsync(x => true);
            Assert.IsNotNull(retailers);
            Assert.IsFalse(retailers.Any());
        }

        [TestMethod]
        public async Task DeleteMissingTest()
        {
            await _factory!.Retailers.DeleteAsync(-1);
            var retailers = await _factory!.Retailers.ListAsync(x => true);
            Assert.AreEqual(1, retailers!.Count);
            Assert.AreEqual(Name, retailers.First().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(RetailerInUseException))]
        public async Task DeleteInUseTest()
        {
            // Add an album that uses the retailer
            var artist = await _factory!.Artists.AddAsync(ArtistName);
            var genre = await _factory.Genres.AddAsync(Genre, false);
            await _factory.Albums.AddAsync(artist.Id, genre.Id, AlbumTitle, Released, CoverUrl, false, null, null, _retailerId);

            // Now try to delete the retailer - this should raise an exception
            await _factory!.Retailers.DeleteAsync(_retailerId);
        }
    }
}
