using ClosedXML.Excel;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.BusinessLogic.DataExchange.Playlists
{
    public class PlaylistXlsxExporter : PlaylistExporterBase, IPlaylistExporter
    {
        private const string WorksheetName = "Playlist";

        private IXLWorksheet? _worksheet = null;

#pragma warning disable CS8618
        internal PlaylistXlsxExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export the playlist to an XLSX file
        /// </summary>
        /// <param name="file"></param>
        public void Export(string file, Playlist playlist)
        {
            // Create a new Excel Workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet to contain the data
                _worksheet = workbook.Worksheets.Add(WorksheetName);

                // Iterate over the equipment register, calling the row addition methods. This builds the spreadsheet
                // in memory
                IterateOverPlaylist(playlist);

                // Save the workbook to the specified file
                workbook.SaveAs(file);
            }
        }

        /// <summary>
        /// Add the headers to the CSV file
        /// </summary>
        /// <param name="headers"></param>
        protected override void AddHeaders(IEnumerable<string> headers)
        {
            var columnNumber = 1;
            foreach (var header in headers)
            {
                _worksheet!.Cell(1, columnNumber).Value = header;
                columnNumber++;
            }
        }

        /// <summary>
        /// Add a playlist item to the XLSX file
        /// </summary>
        /// <param name="item"></param>
        /// <param name="recordCount"></param>
        protected override void AddPlaylistItem(FlattenedPlaylistItem item, int recordCount)
        {
            var row = recordCount + 1;
            _worksheet!.Cell(row, 1).Value = item.Position.ToString();
            _worksheet!.Cell(row, 2).Value = item.ArtistName ?? "";
            _worksheet!.Cell(row, 3).Value = item.AlbumTitle ?? "";
            _worksheet!.Cell(row, 4).Value = item.PlayingTime ?? "";
        }

        /// <summary>
        /// Method to add the total playing time to the output
        /// </summary>
        /// <param name="formattedPlayingTime"></param>
        protected override void AddPlayingTime(string formattedPlayingTime, int recordCount)
        {
            var row = recordCount + 1;
            _worksheet!.Cell(row, 4).Value = formattedPlayingTime ?? "";
        }
    }
}
