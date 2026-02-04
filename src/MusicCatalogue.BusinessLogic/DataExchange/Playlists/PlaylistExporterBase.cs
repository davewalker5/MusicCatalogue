using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.BusinessLogic.DataExchange.Playlists
{
    public abstract class PlaylistExporterBase : DataExchangeBase
    {
        private readonly string[] ColumnHeaders =
        {
            "#",
            "Artist",
            "Album",
            "Playing Time"
        };

        public event EventHandler<PlaylistDataExchangeEventArgs>? PlaylistItemExport;

        protected PlaylistExporterBase(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Method to add headers to the output
        /// </summary>
        /// <param name="headers"></param>
        protected abstract void AddHeaders(IEnumerable<string> headers);

        /// <summary>
        /// Method to add a new playlist item to the output
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddPlaylistItem(FlattenedPlaylistItem equipment, int recordNumber);

        /// <summary>
        /// Method to add the total playing time to the output
        /// </summary>
        /// <param name="formattedPlayingTime"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddPlayingTime(string formattedPlayingTime, int recordNumber);

        /// <summary>
        /// Iterate over a playlist calling the methods supplied by the child class to add
        /// headers and each playlist entry the output
        /// </summary>
        /// <param name="playlist"></param>
        protected void IterateOverPlaylist(Playlist playlist)
        {
            // Call the method, supplied by the child class, to add the headers to the output
            AddHeaders(ColumnHeaders);

            // Initialise the record count
            int count = 0;

            // Iterate over the playlist
            for (int i = 0; i < playlist.Albums.Count; i++)
            {
                // Construct a flattened record for this item of equipment
                var flattened = new FlattenedPlaylistItem
                {
                    Position = i + 1,
                    ArtistName = playlist.Albums[i].Artist!.Name,
                    AlbumTitle = playlist.Albums[i].Title,
                    PlayingTime = playlist.Albums[i].FormattedPlayingTime
                };

                // Call the method to add this album to the file
                count++;
                AddPlaylistItem(flattened, count);

                // Raise the equipment exported event
                PlaylistItemExport?.Invoke(this, new PlaylistDataExchangeEventArgs { RecordCount = count, Item = flattened });
            }

            // Finally, call the method, supplied by the child class, to add the total playing time
            // to the output
            AddPlayingTime(playlist.FormattedPlayingTime, playlist.Albums.Count + 1);
        }
    }
}
