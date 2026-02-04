using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Extensions;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class Playlist : MusicCatalogueEntityBase
    {
        public List<Album> Albums { get; set; } = [];
        public long TotalPlayingTime => Albums.Sum(x => x.PlayingTime);
        public string FormattedPlayingTime => DurationExtensions.DurationToFormattedPlayingTime(TotalPlayingTime);
    }
}