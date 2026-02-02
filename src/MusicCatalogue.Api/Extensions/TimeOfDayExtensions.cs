using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Extensions
{
    public static class TimeOfDayExtensions
    {
        /// <summary>
        /// Convert a playlist type to a descriptive string
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public static string ToName(this TimeOfDay type)
        {
            return type switch
            {
                TimeOfDay.Morning => "Morning",
                TimeOfDay.Afternoon => "Afternoon",
                TimeOfDay.Evening => "Evening",
                TimeOfDay.Late => "Late Night",
                _ => "",
            };
        }
    }
}