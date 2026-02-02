using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class DataImport
    {
        private readonly IMusicCatalogueFactory _factory;

        public DataImport(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Import the data held in the specified CSV file
        /// </summary>
        /// <param name="albumName"></param>
        public void Import(string file)
        {
            _factory.Logger.LogMessage(Severity.Info, $"Importing {file} ...");

            try
            {
                // Register a handler for the "track imported" event and import the file 
                _factory.Importer.TrackImport += OnTrackImported;
                Task.Run(() => _factory.Importer.Import(file)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import error: {ex.Message}");
                _factory.Logger.LogMessage(Severity.Info, $"Import error: {ex.Message}");
                _factory.Logger.LogException(ex);
            }
            finally
            {
                // Un-register the event handler
                _factory.Importer.TrackImport -= OnTrackImported;
            }
        }

        /// <summary>
        /// Handler called when a track is imported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTrackImported(object? sender, TrackDataExchangeEventArgs e)
        {
            if (e.Track != null)
            {
                Console.WriteLine($"Imported {e.Track.ArtistName}, {e.Track.AlbumTitle} - {e.Track.TrackNumber} : {e.Track.Title}");
            }
        }
    }
}
