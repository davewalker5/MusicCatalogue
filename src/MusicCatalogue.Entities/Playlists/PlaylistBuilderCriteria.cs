using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class PlaylistBuilderCriteria : MusicCatalogueEntityBase
    {
        public PlaylistType Type { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public int? CurrentArtistId { get; set; }
        public int NumberOfEntries { get; set; }

#warning Playlist export will be replaced with a saved session export
        public string? FileName { get; set; }

        public List<int> IncludedGenreIds { get; set; } = [];
        public List<int> ExcludedGenreIds { get; set; } = [];
    }
}