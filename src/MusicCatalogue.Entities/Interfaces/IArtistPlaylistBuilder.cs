using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IPlaylistBuilder
    {
        Task<List<PlaylistArtist>> BuildPlaylist(PlaylistType mode, TimeOfDay timeOfDay, int n);
        List<PlaylistArtist> BuildPlaylist(IEnumerable<Artist> artists, PlaylistType mode, TimeOfDay timeOfDay, int n);
        Task<List<Album>> PickPlaylistAlbums(IEnumerable<PlaylistArtist> artists);
    }
}