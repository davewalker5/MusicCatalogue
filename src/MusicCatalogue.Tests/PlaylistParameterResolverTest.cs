using MusicCatalogue.BusinessLogic.Playlists;
using MusicCatalogue.Entities.Extensions;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class PlaylistParameterResolverTest
    {
        [DataTestMethod]
        [DataRow(PlaylistType.Curated, TimeOfDay.Morning)]
        [DataRow(PlaylistType.Curated, TimeOfDay.Afternoon)]
        [DataRow(PlaylistType.Curated, TimeOfDay.Evening)]
        [DataRow(PlaylistType.Curated, TimeOfDay.Late)]
        [DataRow(PlaylistType.Normal, TimeOfDay.Morning)]
        [DataRow(PlaylistType.Normal, TimeOfDay.Afternoon)]
        [DataRow(PlaylistType.Normal, TimeOfDay.Evening)]
        [DataRow(PlaylistType.Normal, TimeOfDay.Late)]
        public void ResolveReturnsUsableParameters(PlaylistType type, TimeOfDay timeOfDay)
        {
            var result = PlaylistParameterResolver.Resolve(type, timeOfDay, 42, 1);

            Assert.AreEqual(timeOfDay, result.TimeOfDay);
            Assert.AreEqual(42, result.CurrentArtistId);
            Assert.AreEqual(3, result.NumberOfEntries);
            Assert.IsTrue(result.TopK >= 3);
            Assert.IsTrue(result.Temperature > 0);
            Assert.IsTrue(result.TransitionPenalty > 0);
            Assert.AreEqual(0.55, result.StyleWeight);
            Assert.AreEqual(0.45, result.MoodWeight);
        }

        [TestMethod]
        public void ResolveRejectsUnknownPlaylistType()
            => Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                PlaylistParameterResolver.Resolve((PlaylistType)99, TimeOfDay.Morning, null, 5));

        [DataTestMethod]
        [DataRow(-1.0, 0.0)]
        [DataRow(0.5, 0.5)]
        [DataRow(2.0, 1.0)]
        public void ClampReturnsValueWithinRange(double value, double expected)
            => Assert.AreEqual(expected, NumberRangeExtensions.Clamp(value, 0, 1));

        [DataTestMethod]
        [DataRow(-1, 0)]
        [DataRow(5, 5)]
        [DataRow(20, 10)]
        public void ClampIntegerReturnsValueWithinRange(int value, int expected)
            => Assert.AreEqual(expected, NumberRangeExtensions.ClampInteger(value, 0, 10));
    }
}
