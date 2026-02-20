using ClosedXML.Excel;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.DataExchange.Sessions
{
    public class SessionXlsxExporter : SessionExporterBase, ISessionExporter
    {
        private const string WorksheetName = "Session";

        private IXLWorksheet? _worksheet = null;

#pragma warning disable CS8618
        internal SessionXlsxExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export the session to an XLSX file
        /// </summary>
        /// <param name="file"></param>
        public void Export(string file, Session session)
        {
            // Create a new Excel Workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet to contain the data
                _worksheet = workbook.Worksheets.Add(WorksheetName);

                // Iterate over the session albums, calling the row addition methods. This builds the spreadsheet
                // in memory
                IterateOverSession(session);

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
        /// Add a session albun to the XLSX file
        /// </summary>
        /// <param name="album"></param>
        /// <param name="recordCount"></param>
        protected override void AddSessionAlbum(FlattenedSessionAlbum album, int recordCount)
        {
            var row = recordCount + 1;
            _worksheet!.Cell(row, 1).Value = album.Position.ToString();
            _worksheet!.Cell(row, 2).Value = album.ArtistName ?? "";
            _worksheet!.Cell(row, 3).Value = album.AlbumTitle ?? "";
            _worksheet!.Cell(row, 4).Value = album.PlayingTime ?? "";
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
