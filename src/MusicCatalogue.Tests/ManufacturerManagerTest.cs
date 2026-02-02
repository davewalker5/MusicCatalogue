using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ManufacturerManagerTest
    {
        private const string Name = "Audio-Technica";
        private const string UpdatedName = "Marantz";

        private IMusicCatalogueFactory? _factory = null;
        private int _manufacturerId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context, new MockFileLogger());

            _manufacturerId = Task.Run(() => _factory!.Manufacturers.AddAsync(Name)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            var manufacturers = await _factory!.Manufacturers.ListAsync(x => true);
            Assert.AreEqual(1, manufacturers.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var manufacturer = await _factory!.Manufacturers.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(manufacturer);
            Assert.IsTrue(manufacturer.Id > 0);
            Assert.AreEqual(Name, manufacturer.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var manufacturer = await _factory!.Manufacturers.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(manufacturer);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var manufacturers = await _factory!.Manufacturers.ListAsync(x => true);
            Assert.AreEqual(1, manufacturers!.Count);
            Assert.AreEqual(Name, manufacturers.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var manufacturers = await _factory!.Manufacturers.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, manufacturers!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _factory!.Manufacturers.UpdateAsync(_manufacturerId, UpdatedName);
            var manufacturer = await _factory!.Manufacturers.GetAsync(a => a.Id == _manufacturerId);
            Assert.IsNotNull(manufacturer);
            Assert.IsTrue(manufacturer.Id > 0);
            Assert.AreEqual(UpdatedName, manufacturer.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Manufacturers.DeleteAsync(_manufacturerId);
            var manufacturers = await _factory!.Manufacturers.ListAsync(a => true);
            Assert.AreEqual(0, manufacturers!.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ManufacturerInUseException))]
        public async Task CannotDeleteWithEquipmentTest()
        {
            var context = _factory!.Context as MusicCatalogueDbContext;
            var equipmentType = new EquipmentType { Name = "Turntable" };
            await context!.EquipmentTypes.AddAsync(equipmentType);
            await context!.SaveChangesAsync();

            var equipment = new Equipment { Description = "Turntable", EquipmentTypeId = equipmentType.Id, ManufacturerId = _manufacturerId };
            await context!.Equipment.AddAsync(equipment);
            await context!.SaveChangesAsync();

            await _factory!.Manufacturers.DeleteAsync(_manufacturerId);
        }
    }
}