using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.BusinessLogic.Playlists;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ArtistSimilarityCalculatorTest
    {
        private static readonly SimilarityWeights Weights = new()
        {
            EnergyWeight = 1,
            IntimacyWeight = 1,
            WarmthWeight = 1,
            MoodWeight = 2
        };

        [TestMethod]
        public void ClosestArtistsAreOrderedByStyleThenMood()
        {
            var target = Artist(1, "Target", 3, 3, 3, 1, 2);
            var sameStyleSharedMood = Artist(2, "Shared", 3, 3, 3, 2, 3);
            var sameStyleNoMood = Artist(3, "No moods", 3, 3, 3);
            var differentStyle = Artist(4, "Different", 5, 5, 5, 1, 2);
            var calculator = CreateCalculator();

            var result = calculator.GetClosestArtists(
                [target, differentStyle, sameStyleNoMood, sameStyleSharedMood], Weights, target, 3);

            CollectionAssert.AreEqual(
                new[] { "Shared", "No moods", "Different" },
                result.Select(x => x.Artist.Name).ToArray());
            Assert.AreEqual(1, result[0].SharedMoods);
            Assert.AreEqual(2.0 / 3.0, result[0].MoodDistance, 0.0001);
            Assert.IsTrue(result.All(x => x.Similarity >= 0 && x.Similarity <= 100));
        }

        [TestMethod]
        public void IdenticalArtistScoresOneHundredPercent()
        {
            var target = Artist(1, "Target", 2, 2, 2);
            var identical = Artist(2, "Identical", 2, 2, 2);

            var result = CreateCalculator().GetClosestArtists([target, identical], Weights, target, 1);

            Assert.AreEqual(100, result.Single().Similarity);
            Assert.AreEqual(0, result.Single().Distance);
        }

        [TestMethod]
        public void TargetCanBeIncludedAndResultsAreLimited()
        {
            var target = Artist(1, "Target", 1, 1, 1, 1);
            var result = CreateCalculator().GetClosestArtists(
                [target, Artist(2, "Other", 5, 5, 5, 2)], Weights, target.Id, 1, false);

            Assert.AreEqual(target.Id, result.Single().Artist.Id);
        }

        [TestMethod]
        public void MissingTargetThrowsHelpfulException()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
                CreateCalculator().GetClosestArtists([Artist(1, "One", 1, 1, 1)], Weights, 99, 1));

            StringAssert.Contains(exception.Message, "99");
        }

        [TestMethod]
        public void EmptyInputOrNonPositiveCountReturnsNoMatches()
        {
            var calculator = CreateCalculator();
            var target = Artist(1, "Target", 1, 1, 1);

            Assert.AreEqual(0, calculator.GetClosestArtists([], Weights, target, 5).Count);
            Assert.AreEqual(0, calculator.GetClosestArtists([target], Weights, target, 0).Count);
        }

        [TestMethod]
        public async Task AsyncOverloadLoadsArtistsFromFactory()
        {
            var context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            context.Artists.AddRange(
                Artist(1, "Target", 1, 1, 1),
                Artist(2, "Close", 2, 1, 1),
                Artist(3, "Far", 5, 5, 5));
            await context.SaveChangesAsync();
            var calculator = new ArtistSimilarityCalculator(new MusicCatalogueFactory(context, new MockFileLogger()));

            var result = await calculator.GetClosestArtistsAsync(Weights, 1, 1);

            Assert.AreEqual("Close", result.Single().Artist.Name);
        }

        private static ArtistSimilarityCalculator CreateCalculator()
        {
            var context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            return new ArtistSimilarityCalculator(new MusicCatalogueFactory(context, new MockFileLogger()));
        }

        private static Artist Artist(int id, string name, int energy, int intimacy, int warmth, params int[] moodIds)
            => new()
            {
                Id = id,
                Name = name,
                Energy = energy,
                Intimacy = intimacy,
                Warmth = warmth,
                Moods = moodIds.Select((moodId, index) => new ArtistMood
                {
                    Id = id * 10 + index,
                    ArtistId = id,
                    MoodId = moodId
                }).ToList()
            };
    }
}
