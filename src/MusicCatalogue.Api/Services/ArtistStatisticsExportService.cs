using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.Logic.DataExchange.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class ArtistStatisticsExportService : BackgroundQueueProcessor<ArtistStatisticsExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public ArtistStatisticsExportService(
            ILogger<BackgroundQueueProcessor<ArtistStatisticsExportWorkItem>> logger,
            IBackgroundQueue<ArtistStatisticsExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the artist statistics report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(ArtistStatisticsExportWorkItem item, IMusicCatalogueFactory factory)
        {
            // Get the report data
            MessageLogger.LogInformation("Retrieving the artist statistics report for export");
            var records = await factory.ArtistStatistics.GenerateReportAsync(item.WishList, 1, int.MaxValue);

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.ReportsExportPath, item.FileName);

            // Export the report
            var exporter = new CsvExporter<ArtistStatistics>();
            exporter.Export(records, filePath, ',');
            MessageLogger.LogInformation("Artist statistics report export completed");
        }
    }
}