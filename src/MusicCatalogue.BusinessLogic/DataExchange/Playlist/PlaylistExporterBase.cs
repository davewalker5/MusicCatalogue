using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.DataExchange.Playlist
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
        /// Method to add a new flattened equipment record to the output
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddPlaylist(FlattenedPlaylistItem equipment, int recordNumber);

        /// <summary>
        /// Iterate over a playlist calling the methods supplied by the child class to add
        /// headers and to add each playlist entry the output
        /// </summary>
        /// <param name="playlist"></param>
        protected void IterateOverPlaylist(IList<Album> playlist)
        {
            // Call the method, supplied by the child class, to add the headers to the output
            AddHeaders(ColumnHeaders);

            // Initialise the record count
            int count = 0;

            // Iterate over the playlist
            for (int i = 0; i < playlist.Count; i++)
            {
                // Construct a flattened record for this item of equipment
                var flattened = new FlattenedPlaylistItem
                {
                    Position = i + 1,
                    ArtistName = playlist[i].Artist!.Name,
                    AlbumTitle = playlist[i].Title,
                    PlayingTime = playlist[i].FormattedPlayingTime
                };

                // Call the method to add this item of equipment to the file
                count++;
                AddPlaylist(flattened, count);

                // Raise the equipment exported event
                PlaylistItemExport?.Invoke(this, new PlaylistDataExchangeEventArgs { RecordCount = count, Item = flattened });
            }
        }
    }
}
