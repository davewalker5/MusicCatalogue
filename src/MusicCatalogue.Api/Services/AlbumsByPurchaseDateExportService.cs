using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.BusinessLogic.DataExchange.Generic;

namespace MusicCatalogue.Api.Services
{
    public class AlbumsByPurchaseDateExportService : BackgroundQueueProcessor<AlbumsByPurchaseDateExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public AlbumsByPurchaseDateExportService(
            ILogger<BackgroundQueueProcessor<AlbumsByPurchaseDateExportWorkItem>> logger,
            IBackgroundQueue<AlbumsByPurchaseDateExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the albums by purchase date report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(AlbumsByPurchaseDateExportWorkItem item, IMusicCatalogueFactory factory)
        {
            // Get the report data
            MessageLogger.LogInformation("Retrieving the albums by purchase date report for export");
            var records = await factory.AlbumsByPurchaseDate.GenerateReportAsync(item.Year, item.Month, 1, 1, int.MaxValue);

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.ReportsExportPath, item.FileName);

            // Export the report
            var exporter = new CsvExporter<AlbumByPurchaseDate>();
            exporter.Export(records, filePath, ',');
            MessageLogger.LogInformation("Albums by purchase date report export completed");
        }
    }
}