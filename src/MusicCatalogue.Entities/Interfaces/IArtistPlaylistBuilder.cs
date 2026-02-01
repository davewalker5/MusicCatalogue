using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistPlaylistBuilder
    {
        Task<List<ArtistPlaylistItem>> BuildNormalArtistPlaylist(TimeOfDay timeOfDay, int n);
        List<ArtistPlaylistItem> BuildNormalArtistPlaylist(IEnumerable<Artist> artists, TimeOfDay timeOfDay, int n);
        Task<List<ArtistPlaylistItem>> BuildCuratedArtistPlaylist(TimeOfDay timeOfDay, int n);
        List<ArtistPlaylistItem> BuildCuratedArtistPlaylist(IEnumerable<Artist> artists, TimeOfDay timeOfDay, int n);
    }
}