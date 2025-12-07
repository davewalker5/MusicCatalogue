using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class CatalogueExportService : BackgroundQueueProcessor<CatalogueExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public CatalogueExportService(
            ILogger<BackgroundQueueProcessor<CatalogueExportWorkItem>> logger,
            IBackgroundQueue<CatalogueExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the catalogue
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(CatalogueExportWorkItem item, IMusicCatalogueFactory factory)
        {
            MessageLogger.LogInformation("Retrieving tracks for export");

            // Use the file extension to determine which exporter to use
            var extension = Path.GetExtension(item.FileName).ToLower();
            ITrackExporter? exporter = extension == ".xlsx" ? factory.CatalogueXlsxExporter : factory.CatalogueCsvExporter;

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.CatalogueExportPath, item.FileName);

            // Export the catalogue
            await exporter.Export(filePath);
            MessageLogger.LogInformation("Catalogue export completed");
        }
    }
}
