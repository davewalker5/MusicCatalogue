using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using System.Text;

namespace MusicCatalogue.BusinessLogic.DataExchange.Sessions
{
    public class SessionCsvExporter : SessionExporterBase, ISessionExporter
    {
        private StreamWriter? _writer = null;

#pragma warning disable CS8618
        internal SessionCsvExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export a session to a CSV file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="session"></param>
        public void Export(string file, Session session)
        {
            // Open the CSV file
            using (_writer = new(file, false, Encoding.UTF8))
            {
                // Iterate over the session, calling the row addition methods
                IterateOverSession(session);
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
        /// Add a session album to the CSV file
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_"></param>
        protected override void AddSessionAlbum(FlattenedSessionAlbum item, int _)
            => _writer!.WriteLine(item.ToCsv());

        /// <summary>
        /// Method to add the total playing time to the output
        /// </summary>
        /// <param name="formattedPlayingTime"></param>
        protected override void AddPlayingTime(string formattedPlayingTime, int recordCount)
            => _writer!.WriteLine($",,,{formattedPlayingTime}");
    }
}
