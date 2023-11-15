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
    public class GenreManagerTest
    {
        private const string Name = "Jazz";

        private IGenreManager? _manager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _manager = new MusicCatalogueFactory(context).Genres;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(Name);
            await _manager!.AddAsync(Name);
            var genres = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, genres.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            await _manager!.AddAsync(Name);
            var genre = await _manager!.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(genre);
            Assert.IsTrue(genre.Id > 0);
            Assert.AreEqual(Name, genre.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            await _manager!.AddAsync(Name);
            var genre = await _manager!.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(genre);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            await _manager!.AddAsync(Name);
            var genres = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, genres!.Count);
            Assert.AreEqual(Name, genres.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            await _manager!.AddAsync(Name);
            var genres = await _manager!.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, genres!.Count);
        }
    }
}
