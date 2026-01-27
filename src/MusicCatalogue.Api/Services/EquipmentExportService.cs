using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class EquipmentExportService : BackgroundQueueProcessor<EquipmentExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public EquipmentExportService(
            ILogger<BackgroundQueueProcessor<EquipmentExportWorkItem>> logger,
            IBackgroundQueue<EquipmentExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the equipment register
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(EquipmentExportWorkItem item, IMusicCatalogueFactory factory)
        {
            MessageLogger.LogInformation("Retrieving equipment records for export");

            // Use the file extension to determine which exporter to use
            var extension = Path.GetExtension(item.FileName).ToLower();
            IEquipmentExporter? exporter = extension == ".xlsx" ? factory.EquipmentXlsxExporter : factory.EquipmentCsvExporter;

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.CatalogueExportPath, item.FileName);

            // Export the equipment register
            await exporter.Export(filePath);
            MessageLogger.LogInformation("Equipment register export completed");
        }
    }
}
