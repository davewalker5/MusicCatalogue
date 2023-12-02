using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class EquipmentTypeManagerTest
    {
        private const string Name = "Turntable";
        private const string UpdatedName = "Stylus";

        private IMusicCatalogueFactory? _factory = null;
        private int _equipmentTypeId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            _equipmentTypeId = Task.Run(() => _factory!.EquipmentTypes.AddAsync(Name)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            var manufacturers = await _factory!.EquipmentTypes.ListAsync(x => true);
            Assert.AreEqual(1, manufacturers.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var manufacturer = await _factory!.EquipmentTypes.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(manufacturer);
            Assert.IsTrue(manufacturer.Id > 0);
            Assert.AreEqual(Name, manufacturer.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var manufacturer = await _factory!.EquipmentTypes.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(manufacturer);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var manufacturers = await _factory!.EquipmentTypes.ListAsync(x => true);
            Assert.AreEqual(1, manufacturers!.Count);
            Assert.AreEqual(Name, manufacturers.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var manufacturers = await _factory!.EquipmentTypes.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, manufacturers!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _factory!.EquipmentTypes.UpdateAsync(_equipmentTypeId, UpdatedName);
            var manufacturer = await _factory!.EquipmentTypes.GetAsync(a => a.Id == _equipmentTypeId);
            Assert.IsNotNull(manufacturer);
            Assert.IsTrue(manufacturer.Id > 0);
            Assert.AreEqual(UpdatedName, manufacturer.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.EquipmentTypes.DeleteAsync(_equipmentTypeId);
            var manufacturers = await _factory!.EquipmentTypes.ListAsync(a => true);
            Assert.AreEqual(0, manufacturers!.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(EquipmentTypeInUseException))]
        public async Task CannotDeleteWithEquipmentTest()
        {
            var context = _factory!.Context as MusicCatalogueDbContext;
            var manufacturer = new Manufacturer { Name = "Audio-Technica" };
            await context!.Manufacturers.AddAsync(manufacturer);
            await context!.SaveChangesAsync();

            var equipment = new Equipment { Description = "Turntable", EquipmentTypeId = _equipmentTypeId, ManufacturerId = manufacturer.Id };
            await context!.Equipment.AddAsync(equipment);
            await context!.SaveChangesAsync();

            await _factory!.EquipmentTypes.DeleteAsync(_equipmentTypeId);
        }
    }
}