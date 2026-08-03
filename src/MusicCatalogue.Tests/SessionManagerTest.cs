using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class SessionManagerTest
    {
        private IMusicCatalogueFactory? _factory;
        private int[]? _albumIds;

        [TestInitialize]
        public async Task Initialize()
        {
            var context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context, new MockFileLogger());

            var artist = await _factory.Artists.AddAsync("Session Artist");
            var genre = await _factory.Genres.AddAsync("Jazz", false);
            var first = await _factory.Albums.AddAsync(artist.Id, genre.Id, "First", 2024, null, false, null, null, null);
            var second = await _factory.Albums.AddAsync(artist.Id, genre.Id, "Second", 2025, null, false, null, null, null);
            await _factory.Tracks.AddAsync(first.Id, "First track", 1, 61000);
            await _factory.Tracks.AddAsync(second.Id, "Second track", 1, 122000);
            _albumIds = [first.Id, second.Id];
        }

        [TestMethod]
        public async Task AddAndGetLoadsAlbumsInPositionOrder()
        {
            var createdAt = new DateTime(2026, 7, 1, 18, 30, 0);

            var added = await _factory!.SessionManager.AddAsync(
                createdAt, PlaylistType.Curated, TimeOfDay.Evening, _albumIds!);
            var result = await _factory.SessionManager.GetAsync(x => x.Id == added.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(createdAt, result.CreatedAt);
            Assert.AreEqual(PlaylistType.Curated, result.Type);
            Assert.AreEqual(TimeOfDay.Evening, result.TimeOfDay);
            Assert.AreEqual(2, result.SessionAlbums.Count);
            CollectionAssert.AreEqual(new[] { 1, 2 }, result.SessionAlbums.Select(x => x.Position).ToArray());
            Assert.IsTrue(result.SessionAlbums.All(x => x.Album?.Artist?.Name == "Session Artist"));
            Assert.AreEqual("00:03:03", result.FormattedPlayingTime);
        }

        [TestMethod]
        public async Task ListFiltersOrdersAndPagesSessions()
        {
            await _factory!.SessionManager.AddAsync(new DateTime(2026, 7, 3), PlaylistType.Normal, TimeOfDay.Morning, [_albumIds![0]]);
            var expected = await _factory.SessionManager.AddAsync(new DateTime(2026, 7, 1), PlaylistType.Curated, TimeOfDay.Late, [_albumIds[1]]);
            await _factory.SessionManager.AddAsync(new DateTime(2026, 7, 2), PlaylistType.Curated, TimeOfDay.Evening, _albumIds);

            var firstPage = await _factory.SessionManager.ListAsync(x => x.Type == PlaylistType.Curated, 1, 1);
            var secondPage = await _factory.SessionManager.ListAsync(x => x.Type == PlaylistType.Curated, 2, 1);

            Assert.AreEqual(expected.Id, firstPage.Single().Id);
            Assert.AreEqual(new DateTime(2026, 7, 2), secondPage.Single().CreatedAt);
        }

        [TestMethod]
        public async Task DeleteRemovesSessionAndLinksButNotAlbums()
        {
            var session = await _factory!.SessionManager.AddAsync(
                DateTime.UtcNow, PlaylistType.Normal, TimeOfDay.Afternoon, _albumIds!);

            await _factory.SessionManager.DeleteAsync(session.Id);

            Assert.IsNull(await _factory.SessionManager.GetAsync(x => x.Id == session.Id));
            Assert.AreEqual(0, _factory.Context.Set<MusicCatalogue.Entities.Database.SessionAlbum>().Count());
            Assert.AreEqual(2, (await _factory.Albums.ListAsync(x => true)).Count);
        }

        [TestMethod]
        public async Task DeleteMissingSessionIsANoOp()
        {
            await _factory!.SessionManager.DeleteAsync(999);

            Assert.AreEqual(0, (await _factory.SessionManager.ListAsync(x => true, 1, 10)).Count);
        }
    }
}
