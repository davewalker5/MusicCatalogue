using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.DataExchange
{
    [ExcludeFromCodeCoverage]
    public class FlattenedSessionAlbum
    {
        public int Position { get; set; }
        public string ArtistName { get; set; } = "";
        public string AlbumTitle { get; set; } = "";
        public string PlayingTime { get; set; } = "";

        /// <summary>
        /// Create a representation of the flattened equipment record in CSV format
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
            => $"{Position},{ArtistName},{AlbumTitle},{PlayingTime}";
    }
}
