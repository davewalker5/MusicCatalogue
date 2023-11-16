using DocumentFormat.OpenXml.Bibliography;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
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
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "https://some.host/blue-train.jpg";
        private const string UpdatedGenre = "Swing";

        private IMusicCatalogueFactory? _factory;
        private int _genreId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);
            _genreId = Task.Run(() => _factory.Genres.AddAsync(Genre)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _factory!.Genres.AddAsync(Genre);
            var genres = await _factory!.Genres.ListAsync(x => true);
            Assert.AreEqual(1, genres.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var genre = await _factory!.Genres.GetAsync(a => a.Name == Genre);
            Assert.IsNotNull(genre);
            Assert.IsTrue(genre.Id > 0);
            Assert.AreEqual(Genre, genre.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var genre = await _factory!.Genres.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(genre);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var genres = await _factory!.Genres.ListAsync(x => true);
            Assert.AreEqual(1, genres!.Count);
            Assert.AreEqual(Genre, genres.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var genres = await _factory!.Genres.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, genres!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            var genre = await _factory!.Genres.UpdateAsync(_genreId, UpdatedGenre);
            Assert.IsNotNull(genre);
            Assert.IsTrue(genre.Id > 0);
            Assert.AreEqual(UpdatedGenre, genre.Name);
        }

        [TestMethod]
        public async Task UpdateMissingTest()
        {
            var genre = await _factory!.Genres.UpdateAsync(-1, UpdatedGenre);
            Assert.IsNull(genre);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Genres.DeleteAsync(_genreId);
            var genres = await _factory!.Genres.ListAsync(x => true);
            Assert.IsNotNull(genres);
            Assert.IsFalse(genres.Any());
        }

        [TestMethod]
        public async Task DeleteMissingTest()
        {
            await _factory!.Genres.DeleteAsync(-1);
            var genres = await _factory!.Genres.ListAsync(x => true);
            Assert.AreEqual(1, genres!.Count);
            Assert.AreEqual(Genre, genres.First().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(GenreInUseException))]
        public async Task DeleteInUseTest()
        {
            // Add an album that uses the retailer
            var artist = await _factory!.Artists.AddAsync(ArtistName);
            await _factory.Albums.AddAsync(artist.Id, _genreId, AlbumTitle, Released, CoverUrl, false, null, null, null);

            // Now try to delete the retailer - this should raise an exception
            await _factory!.Genres.DeleteAsync(_genreId);
        }
    }
}
