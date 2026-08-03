using MusicCatalogue.Entities.Extensions;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class DurationExtensionsTest
    {
        [DataTestMethod]
        [DataRow(null, "00:00:00")]
        [DataRow(0L, "00:00:00")]
        [DataRow(1000L, "00:00:01")]
        [DataRow(61000L, "00:01:01")]
        [DataRow(3661000L, "01:01:01")]
        [DataRow(90061000L, "25:01:01")]
        public void DurationToFormattedPlayingTimeFormatsMilliseconds(long? duration, string expected)
            => Assert.AreEqual(expected, DurationExtensions.DurationToFormattedPlayingTime(duration));

        [DataTestMethod]
        [DataRow(null, "00:00")]
        [DataRow(0L, "00:00")]
        [DataRow(1000L, "00:01")]
        [DataRow(61000L, "01:01")]
        [DataRow(3661000L, "61:01")]
        public void TrackDurationToStringFormatsMilliseconds(long? duration, string expected)
            => Assert.AreEqual(expected, DurationExtensions.TrackDurationToString(duration));
    }
}
