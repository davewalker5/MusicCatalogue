using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class DataExport
    {
        private readonly IMusicLogger _logger;
        private readonly IMusicCatalogueFactory _factory;

        public DataExport(IMusicLogger logger, IMusicCatalogueFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        /// <summary>
        /// Export the collection to the specified file
        /// </summary>
        /// <param name="albumName"></param>
        public void Export(string file)
        {
            _logger.LogMessage(Severity.Info, $"Exporting {file} ...");

            // Use the file extension to decide which exporter to use
            var extension = Path.GetExtension(file).ToLower();
            IExporter? exporter = extension == ".xlsx" ? _factory.CatalogueXlsxExporter : _factory.CatalogueCsvExporter;

            try
            {
                // Register a handler for the "track imported" event and import the file 
                exporter!.TrackExport += OnTrackExported;
                Task.Run(() => exporter.Export(file)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export error: {ex.Message}");
                _logger.LogMessage(Severity.Info, $"Export error: {ex.Message}");
                _logger.LogException(ex);
            }
            finally
            {
                // Un-register the event handler
                exporter!.TrackExport -= OnTrackExported;
            }
        }

        /// <summary>
        /// Handler called when a track is imported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTrackExported(object? sender, TrackDataExchangeEventArgs e)
        {
            if (e.Track != null)
            {
                Console.WriteLine($"Exported {e.Track.ArtistName}, {e.Track.AlbumTitle} - {e.Track.TrackNumber} : {e.Track.Title}");
            }
        }
    }
}
