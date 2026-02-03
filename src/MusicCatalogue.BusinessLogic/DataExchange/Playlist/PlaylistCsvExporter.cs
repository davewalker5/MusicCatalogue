using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using System.Text;

namespace MusicCatalogue.BusinessLogic.DataExchange.Playlist
{
    public class PlaylistCsvExporter : PlaylistExporterBase, IPlaylistExporter
    {
        private StreamWriter? _writer = null;

#pragma warning disable CS8618
        internal PlaylistCsvExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export a playlist to a CSV file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="playlist"></param>
        public void Export(string file, IList<Album> playlist)
        {
            // Open the CSV file
            using (_writer = new(file, false, Encoding.UTF8))
            {
                // Iterate over the playlist, calling the row addition methods
                IterateOverPlaylist(playlist);
            }
        }

        /// <summary>
        /// Add the headers to the CSV file
        /// </summary>
        /// <param name="headers"></param>
        protected override void AddHeaders(IEnumerable<string> headers)
        {
            var csvHeaders = string.Join(",", headers);
            _writer!.WriteLine(csvHeaders);
        }

        /// <summary>
        /// Add a playlist item to the CSV file
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_"></param>
        protected override void AddPlaylist(FlattenedPlaylistItem item, int _)
        {
            _writer!.WriteLine(item.ToCsv());
        }
    }
}
