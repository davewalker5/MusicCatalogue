using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Api.Extensions
{
    public static class VocalPresenceExtensions
    {
        /// <summary>
        /// Convert a data exchange type to a descriptive string
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public static string ToName(this VocalPresence type)
        {
            return type switch
            {
                VocalPresence.Unknown => "",
                VocalPresence.Instrumental => "Instrumental",
                VocalPresence.Mixed => "Mixed",
                VocalPresence.VocalLed => "Vocal Led",
                _ => "",
            };
        }
    }
}