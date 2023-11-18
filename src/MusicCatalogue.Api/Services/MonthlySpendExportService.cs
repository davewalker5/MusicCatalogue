using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.Logic.DataExchange.Generic;

namespace MusicCatalogue.Api.Services
{
    public class MonthlySpendExportService : BackgroundQueueProcessor<MonthlySpendExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public MonthlySpendExportService(
            ILogger<BackgroundQueueProcessor<MonthlySpendExportWorkItem>> logger,
            IBackgroundQueue<MonthlySpendExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the monthly spend report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(MonthlySpendExportWorkItem item, IMusicCatalogueFactory factory)
        {
            // Get the report data
            MessageLogger.LogInformation("Retrieving the monthly spending report for export");
            var records = await factory.MonthlySpend.GenerateReportAsync(false, 1, int.MaxValue);

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.ReportsExportPath, item.FileName);

            // Export the report
            var exporter = new CsvExporter<MonthlySpend>();
            exporter.Export(records, filePath, ',');
            MessageLogger.LogInformation("Monthly spending report export completed");
        }
    }
}