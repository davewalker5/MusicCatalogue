using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.Api.Services
{
    public class SessionExportService : BackgroundQueueProcessor<SessionExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public SessionExportService(
            ILogger<BackgroundQueueProcessor<SessionExportWorkItem>> logger,
            IBackgroundQueue<SessionExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the albums by genre report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(SessionExportWorkItem item, IMusicCatalogueFactory factory)
        {
            MessageLogger.LogInformation($"Retrieving saved session {item.SessionId} for export to {item.FileName}");

            // Load the saved session data
            var session = await factory.SessionManager.GetAsync(x => x.Id == item.SessionId);
            if (session != null)
            {
                // Use the file extension to determine which exporter to use
                var extension = Path.GetExtension(item.FileName).ToLower();
                ISessionExporter? exporter = extension == ".xlsx" ? factory.SessionXlsxExporter : factory.SessionCsvExporter;

                // Construct the full path to the export file
                var filePath = Path.Combine(_settings.SessionExportPath, item.FileName);

                // Export the session
                exporter.Export(filePath, session);
                MessageLogger.LogInformation("Saved session export completed");
            }
            else
            {
                MessageLogger.LogError($"Session with ID {item.SessionId} not found");
            }
        }
    }
}