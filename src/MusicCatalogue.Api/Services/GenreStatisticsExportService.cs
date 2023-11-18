using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.Logic.DataExchange.Generic;

namespace MusicCatalogue.Api.Services
{
    public class GenreStatisticsExportService : BackgroundQueueProcessor<GenreStatisticsExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public GenreStatisticsExportService(
            ILogger<BackgroundQueueProcessor<GenreStatisticsExportWorkItem>> logger,
            IBackgroundQueue<GenreStatisticsExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the genre statistics report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(GenreStatisticsExportWorkItem item, IMusicCatalogueFactory factory)
        {
            // Get the report data
            MessageLogger.LogInformation("Retrieving the genre statistics report for export");
            var records = await factory.GenreStatistics.GenerateReportAsync(item.WishList, 1, int.MaxValue);

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.ReportsExportPath, item.FileName);

            // Export the report
            var exporter = new CsvExporter<GenreStatistics>();
            exporter.Export(records, filePath, ',');
            MessageLogger.LogInformation("Genre statistics report export completed");
        }
    }
}