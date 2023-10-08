using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.DataExchange
{
    public abstract class DataExportBase : DataExchangeBase
    {
        private readonly string[] ColumnHeaders =
        {
            "Artist",
            "Album",
            "Genre",
            "Released",
            "Cover Url",
            "Track Number",
            "Track",
            "Duration",
        };

        public event EventHandler<TrackDataExchangeEventArgs>? TrackExport;

        protected DataExportBase(IMusicCatalogueFactory factory) : base(factory)
        {

        }

        /// <summary>
        /// Method to add headers to the output
        /// </summary>
        /// <param name="headers"></param>
        protected abstract void AddHeaders(IEnumerable<string> headers);

        /// <summary>
        /// Method to add a new flattened track to the output
        /// </summary>
        /// <param name="track"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddTrack(FlattenedTrack track, int recordNumber);

        /// <summary>
        /// Iterate over the collection calling the methods supplied by the child class to add
        /// headers and to add each track to the output
        /// </summary>
        protected async Task IterateOverCollection()
        {
            // Call the method, supplied by the child class, to add the headers to the output
            AddHeaders(ColumnHeaders);

            // Initialise the record count
            int count = 0;

            // Retrieve a list of artists and their albums then iterate over the artists
            // and albums
            var artists = await _factory.Artists.ListAsync(x => true);
            foreach (var artist in artists.OrderBy(x => x.Name))
            {
                foreach (var album in artist.Albums.OrderBy(x => x.Title))
                {
                    // Retrieve the track list for this album and iterate over the tracks
                    var tracks = await _factory.Tracks.ListAsync(x => x.AlbumId == album.Id);
                    foreach (var track in tracks.OrderBy(x => x.Number))
                    {
                        // Construct a flattened track
                        var flattened = new FlattenedTrack
                        {
                            ArtistName = artist.Name,
                            AlbumTitle = album.Title,
                            Genre = album.Genre,
                            Released = album.Released,
                            CoverUrl = album.CoverUrl,
                            TrackNumber = track.Number,
                            Title = track.Title,
                            Duration = track.Duration
                        };

                        // Call the method to add this track to the file
                        count++;
                        AddTrack(flattened, count);

                        // Raise the track exported event
                        TrackExport?.Invoke(this, new TrackDataExchangeEventArgs { RecordCount = count, Track = flattened });
                    }
                }
            }
        }
    }
}
