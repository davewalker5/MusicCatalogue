using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class EquipmentDataExchangeTest
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

        [TestInitialize]
        public void Initialise()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            var equipmentTypeId = Task.Run(() => _factory.EquipmentTypes.AddAsync(EquipmentType)).Result.Id;
            var manufacturerId = Task.Run(() => _factory.Manufacturers.AddAsync(Manufacturer)).Result.Id;
            var retailerId = Task.Run(() => _factory.Retailers.AddAsync(Retailer)).Result.Id;
            Task.Run(() => _factory.Equipment.AddAsync(equipmentTypeId, manufacturerId, Description, Model, SerialNumber, null, Purchased, Price, retailerId)).Wait();
        }

        [TestMethod]
        public void ExportCsvTest()
        {
            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "csv");
            Task.Run(() => _factory!.EquipmentCsvExporter.Export(filepath)).Wait();

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }

        [TestMethod]
        public void ExportXlsxTest()
        {
            var filepath = Path.ChangeExtension(Path.GetTempFileName(), "xlsx");
            Task.Run(() => _factory!.EquipmentXlsxExporter.Export(filepath)).Wait();

            var info = new FileInfo(filepath);
            Assert.AreEqual(info.FullName, filepath);
            Assert.IsTrue(info.Length > 0);

            File.Delete(filepath);
        }
    }
}
