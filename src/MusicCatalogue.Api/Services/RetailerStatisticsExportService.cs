using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.BusinessLogic.DataExchange.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class RetailerStatisticsExportService : BackgroundQueueProcessor<RetailerStatisticsExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public RetailerStatisticsExportService(
            ILogger<BackgroundQueueProcessor<RetailerStatisticsExportWorkItem>> logger,
            IBackgroundQueue<RetailerStatisticsExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the retailer statistics report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(RetailerStatisticsExportWorkItem item, IMusicCatalogueFactory factory)
        {
            // Get the report data
            MessageLogger.LogInformation("Retrieving the retailer statistics report for export");
            var records = await factory.RetailerStatistics.GenerateReportAsync(item.WishList, 1, int.MaxValue);

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.ReportsExportPath, item.FileName);

            // Export the report
            var exporter = new CsvExporter<RetailerStatistics>();
            exporter.Export(records, filePath, ',');
            MessageLogger.LogInformation("Retailer statistics report export completed");
        }
    }
}