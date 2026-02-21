using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.DataExchange.Sessions
{
    public abstract class SessionExporterBase : DataExchangeBase
    {
        private readonly string[] ColumnHeaders =
        {
            "#",
            "Artist",
            "Album",
            "Playing Time"
        };

        public event EventHandler<SessionDataExchangeEventArgs>? SessionAlbumExport;

        protected SessionExporterBase(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Method to add headers to the output
        /// </summary>
        /// <param name="headers"></param>
        protected abstract void AddHeaders(IEnumerable<string> headers);

        /// <summary>
        /// Method to add a new session album to the output
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddSessionAlbum(FlattenedSessionAlbum equipment, int recordNumber);

        /// <summary>
        /// Method to add the total playing time to the output
        /// </summary>
        /// <param name="formattedPlayingTime"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddPlayingTime(string formattedPlayingTime, int recordNumber);

        /// <summary>
        /// Iterate over a session calling the methods supplied by the child class to add
        /// headers and each session album to the output
        /// </summary>
        /// <param name="session"></param>
        protected void IterateOverSession(Session session)
        {
            // Call the method, supplied by the child class, to add the headers to the output
            AddHeaders(ColumnHeaders);

            // Initialise the record count
            int count = 0;

            // Iterate over the session albums
            foreach (var sessionAlbum in session.SessionAlbums)
            {
                count++;

                // Construct a flattened record for this album
                var flattened = new FlattenedSessionAlbum
                {
                    Position = count + 1,
                    ArtistName = sessionAlbum.Album!.Artist!.Name,
                    AlbumTitle = sessionAlbum.Album.Title,
                    PlayingTime = sessionAlbum.Album.FormattedPlayingTime
                };

                // Call the method to add this album to the file
                AddSessionAlbum(flattened, count);

                // Raise the equipment exported event
                SessionAlbumExport?.Invoke(this, new SessionDataExchangeEventArgs { RecordCount = count, Item = flattened });
            }

            // Finally, call the method, supplied by the child class, to add the total playing time
            // to the output
            AddPlayingTime(session.FormattedPlayingTime, session.SessionAlbums.Count + 1);
        }
    }
}
