using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class EquipmentManagerTest
    {
        private const string EquipmentType = "Turntable";
        private const string Manufacturer = "Audio-Technica";
        private const string Description = "Turntable and Edifier R1280T Active Speaker Package";
        private const string Model = "AT-LP60X";
        private const string SerialNumber = "872364012";
        private readonly DateTime Purchased = new(2023, 1, 1, 0, 0, 0);
        private const decimal Price = 152.45M;
        private const string Retailer = "Amazon";

        private IMusicCatalogueFactory? _factory;
        private int _equipmentTypeId;
        private int _manufacturerId;
        private int _equipmentId;

        [TestInitialize]
        public void Initialise()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            _equipmentTypeId = Task.Run(() => _factory.EquipmentTypes.AddAsync(EquipmentType)).Result.Id;
            _manufacturerId = Task.Run(() => _factory.Manufacturers.AddAsync(Manufacturer)).Result.Id;
            _equipmentId = Task.Run(() => _factory.Equipment.AddAsync(_equipmentTypeId, _manufacturerId, Description, Model, SerialNumber, null, null, null, null)).Result.Id;
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var equipment = await _factory!.Equipment.GetAsync(x => x.Id == _equipmentId);
            Assert.IsNotNull(equipment);
            Assert.AreEqual(_equipmentId, equipment.Id);
            Assert.AreEqual(Description, equipment.Description);
            Assert.AreEqual(Model, equipment.Model);
            Assert.AreEqual(SerialNumber, equipment.SerialNumber);
            Assert.AreEqual(EquipmentType, equipment.EquipmentType!.Name);
            Assert.AreEqual(Manufacturer, equipment.Manufacturer!.Name);
            Assert.IsNull(equipment.IsWishListItem);
            Assert.IsNull(equipment.Purchased);
            Assert.IsNull(equipment.Price);
            Assert.IsNull(equipment.RetailerId);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var equiment = await _factory!.Equipment.GetAsync(x => x.Description == "Missing");
            Assert.IsNull(equiment);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var equipment = await _factory!.Equipment.ListAsync(x => true);
            Assert.AreEqual(1, equipment!.Count);
            Assert.AreEqual(_equipmentId, equipment.First().Id);
            Assert.AreEqual(Description, equipment.First().Description);
            Assert.AreEqual(Model, equipment.First().Model);
            Assert.AreEqual(SerialNumber, equipment.First().SerialNumber);
            Assert.AreEqual(EquipmentType, equipment.First().EquipmentType!.Name);
            Assert.AreEqual(Manufacturer, equipment.First().Manufacturer!.Name);
            Assert.IsNull(equipment.First().IsWishListItem);
            Assert.IsNull(equipment.First().Purchased);
            Assert.IsNull(equipment.First().Price);
            Assert.IsNull(equipment.First().RetailerId);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var equipment = await _factory!.Equipment.ListAsync(x => x.Description == "Missing");
            Assert.AreEqual(0, equipment!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var retailerId = Task.Run(() => _factory!.Retailers.AddAsync(Retailer)).Result.Id;
            await _factory!.Equipment.UpdateAsync(_equipmentId, _equipmentTypeId, _manufacturerId, Description, Model, SerialNumber, true, Purchased, Price, retailerId);
            var equipment = await _factory!.Equipment.GetAsync(x => x.Id == _equipmentId);
            Assert.IsNotNull(equipment);
            Assert.AreEqual(_equipmentId, equipment.Id);
            Assert.AreEqual(Description, equipment.Description);
            Assert.AreEqual(Model, equipment.Model);
            Assert.AreEqual(SerialNumber, equipment.SerialNumber);
            Assert.AreEqual(EquipmentType, equipment.EquipmentType!.Name);
            Assert.AreEqual(Manufacturer, equipment.Manufacturer!.Name);
            Assert.IsTrue(equipment.IsWishListItem);
            Assert.AreEqual(Purchased, equipment.Purchased);
            Assert.AreEqual(Price, equipment.Price);
            Assert.AreEqual(Retailer, equipment.Retailer!.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Equipment.DeleteAsync(_equipmentId);
            var equipment = await _factory.Equipment.ListAsync(a => true);
            Assert.AreEqual(0, equipment!.Count);
        }
    }
}
