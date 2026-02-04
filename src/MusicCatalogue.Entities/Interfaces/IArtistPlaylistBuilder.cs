using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IPlaylistBuilder
    {
        Task<Playlist> BuildPlaylistAsync(PlaylistType mode, TimeOfDay timeOfDay, int n);
        Task<Playlist> BuildPlaylistAsync(IEnumerable<Artist>? artists, PlaylistType mode, TimeOfDay timeOfDay, int n);
    }
}