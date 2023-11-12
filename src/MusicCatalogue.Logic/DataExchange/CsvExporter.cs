using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using System.Text;

namespace MusicCatalogue.Logic.DataExchange
{
    public class CsvExporter : DataExportBase, IExporter
    {
        private StreamWriter? _writer = null;

#pragma warning disable CS8618
        internal CsvExporter(IMusicCatalogueFactory factory) : base(factory)
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
            // Open the CSV file
            using (_writer = new(file, false, Encoding.UTF8))
            {
                // Iterate over the collection, calling the row addition methods
                await IterateOverCollection();
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
        /// Add a track to the CSV file
        /// </summary>
        /// <param name="track"></param>
        /// <param name="_"></param>
        protected override void AddTrack(FlattenedTrack track, int _)
        {
            _writer!.WriteLine(track.ToCsv());
        }
    }
}
