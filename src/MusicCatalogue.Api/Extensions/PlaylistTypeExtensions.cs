using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Extensions
{
    public static class PlaylistTypeExtensions
    {
        /// <summary>
        /// Convert a playlist type to a descriptive string
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public static string ToName(this PlaylistType type)
        {
            return type switch
            {
                PlaylistType.Normal => "Normal",
                PlaylistType.Curated => "Curated",
                _ => "",
            };
        }
    }
}