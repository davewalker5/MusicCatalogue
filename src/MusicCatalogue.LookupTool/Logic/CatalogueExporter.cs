using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.LookupTool.Entities;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class CatalogueExporter : DataExportBase
    {
        public CatalogueExporter(IMusicLogger logger, IMusicCatalogueFactory factory) : base(logger, factory)
        {
        }

        /// <summary>
        /// Export the music collection to the specified file
        /// </summary>
        /// <param name="file"></param>
        public override void Export(string file)
        {
            Console.WriteLine($"Exporting the music catalogue to {file} ...");

            // Use the file extension to decide which exporter to use
            var extension = Path.GetExtension(file).ToLower();
            ITrackExporter? exporter = extension == ".xlsx" ? Factory.CatalogueXlsxExporter : Factory.CatalogueCsvExporter;

            try
            {
                // Register a handler for the "track imported" event and import the file 
                exporter!.TrackExport += OnTrackExported;
                Task.Run(() => exporter.Export(file)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export error: {ex.Message}");
                Logger.LogMessage(Severity.Info, $"Export error: {ex.Message}");
                Logger.LogException(ex);
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
