using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Search;
using MusicCatalogue.Logic.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class EquipmentSearchManagerTest
    {
        private const string TurntableEquipmentType = "Turntable";
        private const string TurntableManufacturer = "Audio-Technica";
        private const string TurntableDescription = "AT-LP60X";

        private const string AmplifierEquipmentType = "Amplifier";
        private const string AmplifierManufacturer = "Marantz";
        private const string AmplifierDescription = "PM6007";

        private IMusicCatalogueFactory? _factory;

        private int _turntableEquipmentTypeId;
        private int _turntableManufacturerId;

        private int _amplifierEquipmentTypeId;
        private int _amplifierManufacturerId;

        [TestInitialize]
        public void Initialise()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Create the equipment types
            _turntableEquipmentTypeId = Task.Run(() => _factory.EquipmentTypes.AddAsync(TurntableEquipmentType)).Result.Id;
            _amplifierEquipmentTypeId = Task.Run(() => _factory.EquipmentTypes.AddAsync(AmplifierEquipmentType)).Result.Id;

            // Create the euipment manufacturers
            _turntableManufacturerId = Task.Run(() => _factory.Manufacturers.AddAsync(TurntableManufacturer)).Result.Id;
            _amplifierManufacturerId = Task.Run(() => _factory.Manufacturers.AddAsync(AmplifierManufacturer)).Result.Id;

            // Create the equipment records
            Task.Run(() => _factory.Equipment.AddAsync(_turntableEquipmentTypeId, _turntableManufacturerId, TurntableDescription, null, null, null, null, null, null)).Wait();
            Task.Run(() => _factory.Equipment.AddAsync(_amplifierEquipmentTypeId, _amplifierManufacturerId, AmplifierDescription, null, null, true, null, null, null)).Wait();
        }

        [TestMethod]
        public async Task SearchForAllEquipmentTest()
        {
            var equipment = await _factory!.Search.EquipmentSearchAsync(new EquipmentSearchCriteria());
            Assert.IsNotNull(equipment);
            Assert.AreEqual(2, equipment.Count);
        }

        [TestMethod]
        public async Task SearchForMainRegisterTest()
        {
            var criteria = new EquipmentSearchCriteria { WishList = false };
            var equiment = await _factory!.Search.EquipmentSearchAsync(criteria);
            Assert.IsNotNull(equiment);
            Assert.AreEqual(1, equiment.Count);
            Assert.AreEqual(TurntableDescription, equiment[0].Description);
        }

        [TestMethod]
        public async Task SearchForWishListTest()
        {
            var criteria = new EquipmentSearchCriteria { WishList = true };
            var equipment = await _factory!.Search.EquipmentSearchAsync(criteria);
            Assert.IsNotNull(equipment);
            Assert.AreEqual(1, equipment.Count);
            Assert.AreEqual(AmplifierDescription, equipment[0].Description);
        }

        [TestMethod]
        public async Task SearchForEquipmentTypeTest()
        {
            var criteria = new EquipmentSearchCriteria { EquipmentTypeId = _turntableEquipmentTypeId };
            var equipment = await _factory!.Search.EquipmentSearchAsync(criteria);
            Assert.IsNotNull(equipment);
            Assert.AreEqual(1, equipment.Count);
            Assert.AreEqual(TurntableDescription, equipment[0].Description);
        }

        [TestMethod]
        public async Task SearchForManufacturerTest()
        {
            var criteria = new EquipmentSearchCriteria { ManufacturerId = _amplifierManufacturerId };
            var equipment = await _factory!.Search.EquipmentSearchAsync(criteria);
            Assert.IsNotNull(equipment);
            Assert.AreEqual(1, equipment.Count);
            Assert.AreEqual(AmplifierDescription, equipment[0].Description);
        }

        [TestMethod]
        public async Task SearchForCombinationTest()
        {
            var criteria = new EquipmentSearchCriteria
            {
                EquipmentTypeId = _turntableEquipmentTypeId,
                ManufacturerId = _turntableManufacturerId,
                WishList = false
            };
            var equipment = await _factory!.Search.EquipmentSearchAsync(criteria);
            Assert.IsNotNull(equipment);
            Assert.AreEqual(1, equipment.Count);
            Assert.AreEqual(TurntableDescription, equipment[0].Description);
        }

        [TestMethod]
        public async Task NoMatchesTest()
        {
            var criteria = new EquipmentSearchCriteria
            {
                EquipmentTypeId = _amplifierManufacturerId,
                ManufacturerId = _amplifierManufacturerId,
                WishList = false
            };
            var equipment = await _factory!.Search.EquipmentSearchAsync(criteria);
            Assert.IsNull(equipment);
        }
    }
}
