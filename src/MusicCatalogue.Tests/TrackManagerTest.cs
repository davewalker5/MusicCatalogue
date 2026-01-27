using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class TrackManagerTest
    {
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "https://some.host/blue-train.jpg";
        private const string TrackTitle = "Blue Train";
        private const int TrackNumber = 1;
        private const int TrackDuration = 643200;

        private const string UpdatedTrackTitle = "Moment's Notice";
        private const int UpdatedTrackNumber = 2;
        private const int UpdatedTrackDuration = 550760;

        private ITrackManager? _manager = null;
        private int _artistId;
        private int _albumId;
        private int _trackId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            var factory = new MusicCatalogueFactory(context);

            // Set up an artist and album for the tracks to belong to
            _artistId = Task.Run(() => factory.Artists.AddAsync(ArtistName)).Result.Id;
            var genreId = Task.Run(() => factory.Genres.AddAsync(Genre, false)).Result.Id;
            _albumId = Task.Run(() => factory.Albums.AddAsync(_artistId, genreId, AlbumTitle, Released, CoverUrl, false, null, null, null)).Result.Id;

            // Create a track manager and add a test track
            _manager = factory.Tracks;
            _trackId = Task.Run(() => _manager.AddAsync(_albumId, TrackTitle, TrackNumber, TrackDuration)).Result.Id;
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(_albumId, TrackTitle, TrackNumber, TrackDuration);
            var tracks = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, tracks.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var track = await _manager!.GetAsync(t => t.Title == TrackTitle);
            Assert.IsNotNull(track);
            Assert.IsTrue(track.Id > 0);
            Assert.AreEqual(_albumId, track.AlbumId);
            Assert.AreEqual(TrackTitle, track.Title);
            Assert.AreEqual(TrackNumber, track.Number);
            Assert.AreEqual(TrackDuration, track.Duration);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var track = await _manager!.GetAsync(t => t.Title == "Missing");
            Assert.IsNull(track);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var tracks = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, tracks!.Count);
            Assert.AreEqual(TrackTitle, tracks.First().Title);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var tracks = await _manager!.ListAsync(t => t.Title == "Missing");
            Assert.AreEqual(0, tracks!.Count);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _manager!.UpdateAsync(_trackId, _albumId, UpdatedTrackTitle, UpdatedTrackNumber, UpdatedTrackDuration);
            var track = await _manager!.GetAsync(t => t.Id == _trackId);
            Assert.IsNotNull(track);
            Assert.AreEqual(_trackId, track.Id);
            Assert.AreEqual(_albumId, track.AlbumId);
            Assert.AreEqual(UpdatedTrackTitle, track.Title);
            Assert.AreEqual(UpdatedTrackNumber, track.Number);
            Assert.AreEqual(UpdatedTrackDuration, track.Duration);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            await _manager!.DeleteAsync(_trackId);
            var tracks = await _manager!.ListAsync(t => true);
            Assert.AreEqual(0, tracks!.Count);
        }

        [TestMethod]
        public async Task DeleteAllForAlbumTest()
        {
            await _manager!.DeleteAllTracksForAlbumAsync(_albumId);
            var tracks = await _manager!.ListAsync(t => t.AlbumId == _albumId);
            Assert.AreEqual(0, tracks!.Count);

        }
    }
}
