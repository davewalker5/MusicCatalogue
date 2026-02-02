using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ArtistMoodManagerTest
    {
        private const string Artist = "Julie London";
        private const string Mood = "Late Night";

        private IMusicCatalogueFactory? _factory = null;
        private int _artistId;
        private int _moodId;
        private int _mappingId;

        [TestInitialize]
        public async Task TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context, new MockFileLogger());
            _artistId = (await _factory!.Artists.AddAsync(Artist)).Id;
            _moodId = (await _factory!.Moods.AddAsync(Mood, 0, 0, 0, 0)).Id;
            _mappingId = (await _factory!.ArtistMoods.AddAsync(_artistId, _moodId)).Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            _ = await _factory!.ArtistMoods.AddAsync(_artistId, _moodId);
            var artist = await _factory!.Artists.GetAsync(a => a.Id == _artistId, false);
            Assert.AreEqual(1, artist.Moods.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var artist = await _factory!.Artists.GetAsync(a => a.Id == _artistId, false);
            Assert.AreEqual(1, artist.Moods.Count);
            Assert.AreEqual(_moodId, artist.Moods.First().Mood!.Id);
            Assert.AreEqual(Mood, artist.Moods.First().Mood!.Name);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _factory!.ArtistMoods.DeleteAsync(_mappingId);
            var artist = await _factory!.Artists.GetAsync(a => a.Id == _artistId, false);
            Assert.AreEqual(0, artist.Moods.Count);
        }
    }
}