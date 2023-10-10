using ClosedXML.Excel;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.DataExchange
{
    public class XlsxExporter : DataExportBase, IExporter
    {
        private const string WorksheetName = "Music";

        private IXLWorksheet? _worksheet = null;

#pragma warning disable CS8618
        internal XlsxExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export the collection to a CSV file
        /// </summary>
        /// <param name="sightings"></param>
        /// <param name="file"></param>
        public async Task Export(string file)
        {
            // Create a new Excel Workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet to contain the data
                _worksheet = workbook.Worksheets.Add(WorksheetName);

                // Iterate over the collection, calling the row addition methods. This builds the spreadsheet
                // in memory
                await IterateOverCollection();

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
        /// Add a track to the CSV file
        /// </summary>
        /// <param name="track"></param>
        /// <param name="recordCount"></param>
        protected override void AddTrack(FlattenedTrack track, int recordCount)
        {
            var row = recordCount + 1;
            _worksheet!.Cell(row, 1).Value = track.ArtistName ?? "";
            _worksheet!.Cell(row, 2).Value = track.AlbumTitle ?? "";
            _worksheet!.Cell(row, 3).Value = track.Genre ?? "";
            _worksheet!.Cell(row, 4).Value = track.Released?.ToString() ?? "";
            _worksheet!.Cell(row, 5).Value = track.CoverUrl ?? "";
            _worksheet!.Cell(row, 6).Value = track.TrackNumber?.ToString() ?? "";
            _worksheet!.Cell(row, 7).Value = track.Title ?? "";
            _worksheet!.Cell(row, 8).Value = track.FormattedDuration ?? "";
        }
    }
}
