namespace MusicCatalogue.Entities.Extensions
{
    public static class DurationExtensions
    {
        /// <summary>
        /// Convert a track duration to MM:SS
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static string TrackDurationToString(long? duration)
        {
            long seconds = (duration ?? 0) / 1000;
            long minutes = seconds / 60;
            seconds -= 60 * minutes;
            return $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// Calculate a duration to a playing time in HH:MM:SS format
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static string DurationToFormattedPlayingTime(long? duration)
        {
            long seconds = (duration ?? 0)/ 1000;
            long hours = seconds / 3600;
            seconds -= 3600 * hours;
            long minutes = seconds / 60;
            seconds -= 60 * minutes;
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }
    }
}