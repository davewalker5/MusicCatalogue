using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IPlaylistBuilder
    {
        Task<Playlist> BuildPlaylistAsync(
            PlaylistType mode,
            TimeOfDay timeOfDay,
            int? initialArtistId,
            int n,
            IEnumerable<int> includedGenreIds,
            IEnumerable<int> excludedGenreIds);

        Task<Playlist> BuildPlaylistAsync(
            IEnumerable<Artist>? artists,
            PlaylistType mode,
            TimeOfDay timeOfDay,
            int? initialArtistId,
            int n,
            IEnumerable<int> includedGenreIds,
            IEnumerable<int> excludedGenreIds);
    }
}