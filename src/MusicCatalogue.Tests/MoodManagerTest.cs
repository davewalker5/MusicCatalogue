using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class MoodManagerTest
    {
        private const string Name = "Late Night";
        private const string UpdatedName = "Upbeat";

        private IMusicCatalogueFactory? _factory = null;
        private int _moodId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            _moodId = Task.Run(() => _factory!.Moods.AddAsync(Name, 0, 0, 0, 0)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            var moods = await _factory!.Moods.ListAsync(x => true);
            Assert.AreEqual(1, moods.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var mood = await _factory!.Moods.GetAsync(a => a.Name == Name);
            Assert.IsNotNull(mood);
            Assert.IsTrue(mood.Id > 0);
            Assert.AreEqual(Name, mood.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var mood = await _factory!.Moods.GetAsync(a => a.Name == "Missing");
            Assert.IsNull(mood);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var moods = await _factory!.Moods.ListAsync(x => true);
            Assert.AreEqual(1, moods!.Count);
            Assert.AreEqual(Name, moods.First().Name);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var moods = await _factory!.Moods.ListAsync(e => e.Name == "Missing");
            Assert.AreEqual(0, moods!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _factory!.Moods.UpdateAsync(_moodId, UpdatedName, 0, 0, 0, 0);
            var mood = await _factory!.Moods.GetAsync(a => a.Id == _moodId);
            Assert.IsNotNull(mood);
            Assert.IsTrue(mood.Id > 0);
            Assert.AreEqual(UpdatedName, mood.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.Moods.DeleteAsync(_moodId);
            var moods = await _factory!.Moods.ListAsync(a => true);
            Assert.AreEqual(0, moods!.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(MoodInUseException))]
        public async Task CannotDeleteWithArtistsTest()
        {
            var context = _factory!.Context as MusicCatalogueDbContext;
            var artist = new Artist { Name = "Julie London" };
            await context!.Artists.AddAsync(artist);
            await context!.SaveChangesAsync();

            var mapping = new ArtistMood { ArtistId = artist.Id, MoodId = _moodId };
            await context!.ArtistMoods.AddAsync(mapping);
            await context!.SaveChangesAsync();

            await _factory!.Moods.DeleteAsync(_moodId);
        }
    }
}