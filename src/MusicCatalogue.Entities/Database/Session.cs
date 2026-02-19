using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Extensions;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Session : MusicCatalogueEntityBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public PlaylistType Type { get; set; }
        public TimeOfDay TimeOfDay { get; set; }

        public ICollection<SessionAlbum> SessionAlbums { get; set; } = [];

        public long TotalPlayingTime => SessionAlbums?.Sum(x => x.Album?.PlayingTime ?? 0) ?? 0;
        public string FormattedPlayingTime => DurationExtensions.DurationToFormattedPlayingTime(TotalPlayingTime);
    }
}