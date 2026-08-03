using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class PlaylistBuilderTest
    {
        private MusicCatalogueDbContext? _context;
        private MusicCatalogueFactory? _factory;
        private List<Artist>? _artists;

        [TestInitialize]
        public async Task Initialize()
        {
            _context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(_context, new MockFileLogger());

            var jazz = new Genre { Id = 1, Name = "Jazz" };
            var rock = new Genre { Id = 2, Name = "Rock" };
            var bright = new Mood { Id = 1, Name = "Bright", MorningWeight = 1, AfternoonWeight = 2, EveningWeight = 3, LateWeight = 4 };
            var calm = new Mood { Id = 2, Name = "Calm", MorningWeight = 4, AfternoonWeight = 3, EveningWeight = 2, LateWeight = 1 };
            _artists = Enumerable.Range(1, 8).Select(i => new Artist
            {
                Id = i,
                Name = $"Artist {i}",
                Energy = (i % 5) + 1,
                Intimacy = ((i + 1) % 5) + 1,
                Warmth = ((i + 2) % 5) + 1,
                Vocals = (VocalPresence)(i % 3),
                Ensemble = (EnsembleType)(i % 3)
            }).ToList();

            _context.AddRange(jazz, rock, bright, calm);
            _context.Artists.AddRange(_artists);
            await _context.SaveChangesAsync();

            for (var i = 0; i < _artists.Count; i++)
            {
                _context.ArtistMoods.Add(new ArtistMood
                {
                    ArtistId = _artists[i].Id,
                    MoodId = i % 2 == 0 ? bright.Id : calm.Id
                });
                _context.Albums.Add(new Album
                {
                    ArtistId = _artists[i].Id,
                    GenreId = i < 6 ? jazz.Id : rock.Id,
                    Title = $"Album {i + 1}",
                    IsWishListItem = false
                });
            }

            _context.Albums.Add(new Album { ArtistId = _artists[0].Id, GenreId = jazz.Id, Title = "Wish list", IsWishListItem = true });
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        [TestMethod]
        public async Task BuildPlaylistSelectsOwnedAlbumsAndExcludesWishList()
        {
            var result = await _factory!.PlaylistBuilder.BuildPlaylistAsync(
                PlaylistType.Normal, TimeOfDay.Morning, null, 6, [], []);

            Assert.IsTrue(result.Albums.Count > 0);
            Assert.IsTrue(result.Albums.Count <= 6);
            Assert.IsTrue(result.Albums.All(x => x.Artist != null));
            Assert.IsFalse(result.Albums.Any(x => x.Title == "Wish list"));
            Assert.AreEqual(result.Albums.Count, result.Albums.Select(x => x.ArtistId).Distinct().Count());
        }

        [TestMethod]
        public async Task BuildPlaylistFiltersSpecifiedArtistsAndGenres()
        {
            var suppliedArtists = _artists!.Take(7).ToList();

            var result = await _factory!.PlaylistBuilder.BuildPlaylistAsync(
                suppliedArtists, PlaylistType.Curated, TimeOfDay.Evening,
                suppliedArtists[0].Id, 6, [1], [2]);

            Assert.IsTrue(result.Albums.Count > 0);
            Assert.IsTrue(result.Albums.All(x => x.GenreId == 1));
            Assert.IsTrue(result.Albums.All(x => suppliedArtists.Any(a => a.Id == x.ArtistId)));
        }

        [TestMethod]
        public async Task EmptyArtistPoolReturnsEmptyPlaylist()
        {
            _context!.Albums.RemoveRange(_context.Albums);
            await _context.SaveChangesAsync();

            var result = await _factory!.PlaylistBuilder.BuildPlaylistAsync(
                PlaylistType.Curated, TimeOfDay.Late, null, 3, [], []);

            Assert.AreEqual(0, result.Albums.Count);
        }

        [DataTestMethod]
        [DataRow(TimeOfDay.Morning)]
        [DataRow(TimeOfDay.Afternoon)]
        [DataRow(TimeOfDay.Evening)]
        [DataRow(TimeOfDay.Late)]
        public async Task EveryTimeOfDayCanBuildAPlaylist(TimeOfDay timeOfDay)
        {
            var result = await _factory!.PlaylistBuilder.BuildPlaylistAsync(
                PlaylistType.Curated, timeOfDay, null, 3, [], []);

            Assert.IsTrue(result.Albums.Count > 0);
        }
    }
}
